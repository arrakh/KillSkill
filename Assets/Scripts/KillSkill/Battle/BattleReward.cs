using KillSkill.Network;
using Unity.Netcode;
using Unity.VisualScripting;

namespace KillSkill.Battle
{
    public class BattleReward : INetCodeSerializable
    {
        private string resultText;
        private string resourceId;
        private double resourceAmount;

        public BattleReward()
        {
        }

        public string ResultText => resultText;
        public string ResourceId => resourceId;
        public double ResourceAmount => resourceAmount;

        public BattleReward(string resultText, string resourceId, double resourceAmount)
        {
            this.resultText = resultText;
            this.resourceId = resourceId;
            this.resourceAmount = resourceAmount;
        }

        public void Serialize(FastBufferWriter writer)
        {
            writer.WriteValueSafe(resultText);
            writer.WriteValueSafe(resourceId);
            writer.WriteValueSafe(resourceAmount);
        }

        public void Deserialize(FastBufferReader reader)
        {
            reader.ReadValueSafe(out resultText);
            reader.ReadValueSafe(out resourceId);
            reader.ReadValueSafe(out resourceAmount);
        }
    }
}