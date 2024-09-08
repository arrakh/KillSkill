using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arr.EventsSystem;
using Arr.ModulesSystem;
using Arr.ScriptableDatabases;
using Arr.Utils;
using KillSkill.Modules.Network.Events;
using KillSkill.Network;
using KillSkill.Network.Messages;
using KillSkill.SessionData;
using KillSkill.SessionData.Implementations;
using KillSkill.Skills;
using KillSkill.Utility;
using Unity.Collections;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using Object = UnityEngine.Object;
using ParrelSync;

namespace KillSkill.Modules.Network
{
    public class NetworkModule : BaseModule,
        IEventListener<StartJoinEvent>,
        IEventListener<SendMessageEvent>,
        IEventListener<BroadcastMessageEvent>,
        IQueryProvider<IsServerQuery>,
        IQueryProvider<IsClientQuery>
    {
        private NetworkManager networkManager;
        private UnityTransport unityTransport;
        private NetworkIdSessionData networkId;
        //private MessageSizeCache sizeCache = new();

        private NetworkEventQueue networkEventQueue;

        private TypeMapper<INetCodeMessage> messageTypeMapper;
        private Dictionary<Type, Action<ulong, FastBufferReader>> readHandlers = new();

        private TaskCompletionSource<bool> shutdownTcs = new();

        private bool isServerStopping = false;
        
        protected override async Task OnInitialize()
        {
            messageTypeMapper = new TypeMapper<INetCodeMessage>();
            if (messageTypeMapper.LastIndex > ushort.MaxValue)
                throw new Exception("There are more than ushort.Max amount of INetCodeMessage!");
            
            var prefab = PrefabRegistry.Get("network-manager");
            var obj = Object.Instantiate(prefab);
            networkManager = obj.GetComponent<NetworkManager>();
            unityTransport = obj.GetComponent<UnityTransport>();
            
            RegisterMessages();

            networkManager.OnServerStopped += OnServerStopped;

            networkEventQueue = new(networkManager, OnConnectionEvent);
        }


        private void OnConnectionEvent(NetworkManager manager, ConnectionEventData eventData)
        {
            switch (eventData.EventType)
            {
                case ConnectionEvent.ClientConnected: OnClientConnected(eventData.ClientId); break;
                case ConnectionEvent.ClientDisconnected: OnClientDisconnected(eventData.ClientId); break;
            }
        }

        private void OnServerStopped(bool isHost)
        {
            Debug.Log("[NM] SERVER STOPPED");
            isServerStopping = false;
            shutdownTcs?.SetResult(isHost);
        }

        private void RegisterMessages()
        {
            var messageTypes = ReflectionCache.GetAll<INetCodeMessage>();

            foreach (var type in messageTypes)
            {
                readHandlers[type] = (senderId, reader) => OnHandleMessage(type, senderId, reader);
            }
        }

        private void OnHandleMessage(Type type, ulong senderId, FastBufferReader reader)
        {
            var instance = Activator.CreateInstance(type);
            if (instance is not INetCodeMessage msg)
                throw new Exception($"Trying to create INetCodeMessage but {type.Name} is NOT INetCodeMessage");
            msg.Deserialize(reader);

            var eventType = typeof(NetMessageEvent<>).MakeGenericType(type);
            var netEvent = Activator.CreateInstance(eventType, senderId, instance);
            
            Debug.Log($"[NM] WILL FIRE UNSAFE WITH MESSAGE {type.Name}");
            
            GlobalEvents.FireUnsafe(eventType, netEvent);
        }

        private void OnUnnamedMessage(ulong clientId, FastBufferReader reader)
        {
            Debug.Log($"[NM] GOT MESSAGE FROM CLIENT {clientId}");
            reader.ReadValueSafe(out ushort messageIndex);
            var messageType = messageTypeMapper.ToType(messageIndex);

            if (!readHandlers.TryGetValue(messageType, out var readHandler))
            {
                Debug.LogError($"Trying to handle message type {messageType.Name} but could find its read handler");
                return;
            }

            Debug.Log($"[NM] WILL INVOKE HANDLER WITH TYPE {messageType}");
            readHandler.Invoke(clientId, reader);
        }

        protected override async Task OnLoad()
        {
            await base.OnLoad();

            networkId = Session.GetData<NetworkIdSessionData>();

            if (ClonesManager.IsClone())
            {
                unityTransport.SetConnectionData("127.0.0.1", 30303);
                networkManager.StartClient();
            } 
            
            else networkManager.StartHost();
        }

        protected override async Task OnUnload()
        {
            Debug.Log("[NM] UNLOAD, SHUTTING DOWN NETWORK");
            
            networkEventQueue?.Dispose();
            networkManager?.Shutdown();

            await base.OnUnload();
        }


        private void OnClientConnected(ulong id)
        {
            Debug.Log($"[NM] A CLIENT HAS CONNECTED WITH ID {id}, LOCAL IS {networkManager.LocalClientId}");

            if (id != 0 && !id.Equals(networkManager.LocalClientId)) return;
            
            var isServer = networkManager.IsServer;
            networkId.SetClientId(networkManager.LocalClientId);

            Debug.Log($"[NM] {(isServer ? "SERVER " : "")}CLIENT IS CONNECTED WITH ID {networkId.ClientId}");

            networkManager.CustomMessagingManager.OnUnnamedMessage += OnUnnamedMessage;
            
            if (isServer) GlobalEvents.Fire(new HostStartedEvent());
            else GlobalEvents.Fire(new HostJoinedEvent());
        }
        
        private void OnClientDisconnected(ulong id)
        {
            Debug.Log($"[NM] ON CLIENT DISCONNECTED, IS SERVER {networkManager.IsServer}, IS SHUTTING DOWN {networkManager.ShutdownInProgress}");
            if (networkManager.IsServer && networkManager.ShutdownInProgress) return;
            Debug.Log($"[NM] A CLIENT HAS DISCONNECTED WITH ID {id}, LOCAL IS {networkId.ClientId}");

            bool isLocal = id.Equals(networkId.ClientId);
            GlobalEvents.Fire(new ClientDisconnectedEvent(id, isLocal));

            if (isLocal) networkManager.StartHost();
        }
        
        public void OnEvent(StartJoinEvent data)
        {
            JoinAsync(data).CatchExceptions();
        }

        private async Task JoinAsync(StartJoinEvent data)
        {
            Debug.Log($"[NM] SHUTTING DOWN SERVER...");

            shutdownTcs = new();
            
            networkManager.Shutdown(true);

            await shutdownTcs.Task;
            
            Debug.Log($"[NM] SERVER SHUT DOWN! WILL CONNECT TO {data.ip}");

            unityTransport.SetConnectionData(data.ip, 30303);
            var success = networkManager.StartClient();
            
            Debug.Log($"[NM] SUCCESS? {success}");
        }

        public void OnEvent(SendMessageEvent data)
        {
            if (!networkManager.IsClient) return;
            
            var type = data.message.GetType();
            if (!readHandlers.ContainsKey(type))
            {
                Debug.LogError($"Trying to send message but {type.Name} is NOT a registered INetCodeMessage");
                return;
            }

            ushort index = (ushort) messageTypeMapper.ToId(type);
            
            using var writer = new FastBufferWriter(1024, Allocator.Temp);
            writer.WriteValueSafe(index);
            data.message.Serialize(writer);
            
            Debug.Log($"[NM] WILL SEND MESSAGE TYPE {type.Name} TO CLIENT {data.clientId}, LOCAL IS {networkId.ClientId}");
                
            networkManager.CustomMessagingManager.SendUnnamedMessage(data.clientId, writer, 
                NetworkDelivery.ReliableFragmentedSequenced);
        }

        public void OnEvent(BroadcastMessageEvent data)
        {
            if (!networkManager.IsServer) return;
            
            var type = data.message.GetType();
            if (!readHandlers.ContainsKey(type))
            {
                Debug.LogError($"Trying to send message but {type.Name} is NOT a registered INetCodeMessage");
                return;
            }

            ushort index = (ushort) messageTypeMapper.ToId(type);
            
            using var writer = new FastBufferWriter(1024, Allocator.Temp);
            writer.WriteValueSafe(index);
            data.message.Serialize(writer);
            
            Debug.Log($"[NM] WILL BROADCAST TYPE {type.Name}");

            networkManager.CustomMessagingManager.SendUnnamedMessageToAll(writer, 
                NetworkDelivery.ReliableFragmentedSequenced);
        }

        IsServerQuery IQueryProvider<IsServerQuery>.OnQuery() => new (networkManager.IsServer);

        IsClientQuery IQueryProvider<IsClientQuery>.OnQuery() => new (networkManager.IsClient);
    }
}