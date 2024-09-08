using KillSkill.Battle;
using KillSkill.Utility;
using Unity.Netcode;
using Unity.VisualScripting;

namespace KillSkill.Network.Messages.Battle
{
    public class BattleEndNetMessage : INetCodeMessage
    {
        private BattleResultData result;

        public BattleResultData Result => result;
        
        public void Serialize(FastBufferWriter writer)
        {
            writer.Write(result);
        }

        public void Deserialize(FastBufferReader reader)
        {
            reader.Read(out result);
        }
    }
}