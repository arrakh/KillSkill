using System;
using System.Collections.Generic;
using Arr.EventsSystem;
using KillSkill.Modules.Network.Events;
using KillSkill.Network;
using KillSkill.Network.Messages;
using KillSkill.SessionData.Events;
using Unity.Netcode;
using UnityEngine;

namespace KillSkill.SessionData.Implementations
{
    public class NetworkPartySessionData : ISessionData
    {
        private Dictionary<ulong, LobbyUser> party = new();

        public IEnumerable<LobbyUser> Party => party.Values;

        public void Add(LobbyUser user)
        {
            party.Add(user.NetworkId.ClientId, user);
            GlobalEvents.Fire(new SessionUpdatedEvent<NetworkPartySessionData>(this));
        }

        public void Remove(ulong id)
        {
            party.Remove(id);
            GlobalEvents.Fire(new SessionUpdatedEvent<NetworkPartySessionData>(this));
        }

        public void Set(Dictionary<ulong, LobbyUser> newParty)
        {
            party = newParty;
            GlobalEvents.Fire(new SessionUpdatedEvent<NetworkPartySessionData>(this));
        }

        public void Clear() => party.Clear();

        public InformPartyNetMessage GetInformPartyNetMessage() => new (party);

    }
}