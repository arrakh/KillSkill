using System.Threading.Tasks;
using Arr.EventsSystem;
using Arr.ModulesSystem;
using KillSkill.Modules.Network.Events;
using KillSkill.Network;
using KillSkill.Network.Messages;
using KillSkill.SessionData;
using KillSkill.SessionData.Implementations;
using UnityEngine;

namespace KillSkill.Modules.Network
{
    public class NetworkPartyModule : BaseModule,
        IEventListener<HostJoinedEvent>,
        IEventListener<NetMessageEvent<InformLobbyUserNetMessage>>,
        IEventListener<NetMessageEvent<InformPartyNetMessage>>
    {
        private NetworkPartySessionData partySessionData;
        
        protected override async Task OnLoad()
        {
            await base.OnLoad();
            partySessionData = Session.GetData<NetworkPartySessionData>();
            partySessionData.Add(LobbyUser.GetLocal());
            Debug.Log("[NPM] ADDED LOCAL TO PARTY");
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
    }
}