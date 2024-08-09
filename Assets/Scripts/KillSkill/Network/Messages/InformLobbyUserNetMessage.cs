using KillSkill.Utility;
using Unity.Netcode;

namespace KillSkill.Network.Messages
{
    public class InformLobbyUserNetMessage : INetCodeMessage
    {
        private LobbyUser user;
        public LobbyUser User => user;

        public InformLobbyUserNetMessage(){}
        
        public InformLobbyUserNetMessage(LobbyUser user)
        {
            this.user = user;
        }

        public void Serialize(FastBufferWriter writer)
        {
            writer.Write(user);
        }

        public void Deserialize(FastBufferReader reader)
        {
            reader.Read(out user);
        }
    }
}