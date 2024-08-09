using System.Collections.Generic;
using KillSkill.SessionData.Implementations;
using KillSkill.Utility;
using Unity.Netcode;

namespace KillSkill.Network.Messages
{
    public class InformPartyNetMessage : INetCodeMessage
    {
        private Dictionary<ulong, LobbyUser> latestParty;

        public Dictionary<ulong, LobbyUser> LatestParty => latestParty;
        
        public InformPartyNetMessage(){}

        public InformPartyNetMessage(Dictionary<ulong, LobbyUser> latestParty)
        {
            this.latestParty = latestParty;
        }

        public void Serialize(FastBufferWriter writer)
        {
            var length = (byte) latestParty.Count;
            writer.WriteValueSafe(length);

            foreach (var (id, user) in latestParty)
            {
                writer.WriteValueSafe(id);
                writer.Write(user);
            }
        }

        public void Deserialize(FastBufferReader reader)
        {
            reader.ReadValueSafe(out byte length);

            latestParty = new Dictionary<ulong, LobbyUser>(length);
            
            for (int i = 0; i < length; i++)
            {
                reader.ReadValueSafe(out ulong id);
                reader.Read(out LobbyUser user);
                latestParty[id] = user;
            }
        }
    }
}