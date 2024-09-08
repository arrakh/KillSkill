using KillSkill.SessionData;
using KillSkill.SessionData.Implementations;
using KillSkill.Utility;
using Unity.Netcode;

namespace KillSkill.Network
{
    public class LobbyUser : INetCodeSerializable
    {
        public NetworkIdSessionData NetworkId => networkId;

        public SkillsSessionData Skills => skills;
        
        
        
        private NetworkIdSessionData networkId;

        private SkillsSessionData skills;
        
        

        public static LobbyUser GetLocal()
        {
            return new LobbyUser()
            {
                networkId = Session.GetData<NetworkIdSessionData>(),
                skills = Session.GetData<SkillsSessionData>(),
            };
        }


        public void Serialize(FastBufferWriter writer)
        {
            writer.Write(networkId);
            writer.Write(skills);
        }

        public void Deserialize(FastBufferReader reader)
        {
            reader.Read(out networkId);
            reader.Read(out skills);
        }
    }
}