using System.Threading.Tasks;
using Arr.EventsSystem;
using Arr.ModulesSystem;
using KillSkill.Modules.Network.Events;
using KillSkill.Network;
using KillSkill.Network.Messages;
using KillSkill.SessionData;
using KillSkill.SessionData.Implementations;
using Unity.Netcode;
using UnityEngine;

namespace KillSkill.Modules.Network
{
    public class NetworkPartyModule : BaseModule,
        IEventListener<HostJoinedEvent>, 
        IEventListener<HostStartedEvent>, 
        IEventListener<ClientDisconnectedEvent>,
        IEventListener<NetMessageEvent<InformLobbyUserNetMessage>>,
        IEventListener<NetMessageEvent<InformPartyNetMessage>>
    {
        private NetworkPartySessionData partySessionData;

        protected override async Task OnInitialize()
        {
            await base.OnInitialize();
            partySessionData = Session.GetData<NetworkPartySessionData>();
        }

        public void OnEvent(HostJoinedEvent data)
        {
            if (Net.IsServer()) return;
            Debug.Log("[NPM] WILL SEND INFORM LOBBY USER");
            Net.Client.Send(new InformLobbyUserNetMessage(LobbyUser.GetLocal()));
        }

        public void OnEvent(NetMessageEvent<InformLobbyUserNetMessage> data)
        {
            Debug.Log("[NPM] GOT INFORM LOBBY USER, WILL SEND INFORM PARTY");

            partySessionData.Add(data.message.User);
            Net.Server.Broadcast(partySessionData.GetInformPartyNetMessage());
        }

        public void OnEvent(NetMessageEvent<InformPartyNetMessage> data)
        {
            if (Net.IsServer()) return;
            Debug.Log("[NPM] GOT INFORM PARTY, WILL SET PARTY");
            partySessionData.Set(data.message.LatestParty);
        }

        public void OnEvent(ClientDisconnectedEvent data)
        {
            if (data.isLocal) return;
            
            Debug.Log($"[NPM] CLIENT {data.clientId} HAS DISCONNECTED, WILL UPDATE PARTY, IS IT SERVER? {Net.IsServer()} || {NetworkManager.Singleton.IsServer}");
            
            if (!Net.IsServer()) return; //shouldn't happen anyway
            
            partySessionData.Remove(data.clientId);
            Net.Server.Broadcast(partySessionData.GetInformPartyNetMessage());
        }

        public void OnEvent(HostStartedEvent data)
        {
            if (!Net.IsServer()) return; //shouldn't happen anyway
            
            partySessionData.Clear();
            partySessionData.Add(LobbyUser.GetLocal());
            Debug.Log("[NPM] HOST STARTED, ADDED LOCAL TO PARTY");
        }
    }
}