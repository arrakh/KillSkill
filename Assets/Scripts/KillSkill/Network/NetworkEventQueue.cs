using System;
using System.Collections.Generic;
using Arr.Utils;
using Unity.Netcode;

namespace KillSkill.Network
{
    public class NetworkEventQueue : IDisposable
    {
        private NetworkManager networkManager;
        private Action<NetworkManager, ConnectionEventData> onConnectionEvent;
        private Queue<Action> onConnectionEventQueue = new();

        private bool disposed = false;

        public NetworkEventQueue(NetworkManager networkManager, Action<NetworkManager, ConnectionEventData> onConnectionEvent)
        {
            this.networkManager = networkManager;
            this.onConnectionEvent = onConnectionEvent;

            networkManager.OnConnectionEvent += EnqueueConnectionEvent;

            UnityEvents.onUpdate += OnUnityUpdate;
        }

        private void OnUnityUpdate()
        {
            while (onConnectionEventQueue.TryDequeue(out var action))
                action.Invoke();
        }

        private void EnqueueConnectionEvent(NetworkManager mngr, ConnectionEventData data)
            => onConnectionEventQueue.Enqueue(() => onConnectionEvent.Invoke(mngr, data));

        public void Dispose()
        {
            if (disposed) return;
                        
            UnityEvents.onUpdate -= OnUnityUpdate;
            networkManager.OnConnectionEvent -= EnqueueConnectionEvent;

            disposed = true;
        }
    }
}