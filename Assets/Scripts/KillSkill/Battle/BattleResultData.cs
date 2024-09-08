using System.Collections.Generic;
using KillSkill.Network;
using KillSkill.Utility;
using Unity.Netcode;

namespace KillSkill.Battle
{
    public class BattleResultData : INetCodeSerializable
    {
        private bool playerWon;
        private ICollection<BattleReward> rewards;
        private ICollection<string> milestones;

        public BattleResultData() { }

        public bool PlayerWon => playerWon;

        public ICollection<BattleReward> Rewards => rewards;
        public ICollection<string> Milestones => milestones;

        public BattleResultData(bool playerWon, ICollection<BattleReward> rewards, ICollection<string> milestones)
        {
            this.playerWon = playerWon;
            this.rewards = rewards;
            this.milestones = milestones;
        }

        public void Serialize(FastBufferWriter writer)
        {
            writer.WriteValueSafe(playerWon);
            writer.WriteValueSafe(rewards);
            writer.WriteValueSafe(milestones);
        }

        public void Deserialize(FastBufferReader reader)
        {
            reader.ReadValueSafe(out playerWon);
            reader.ReadValueSafe(out rewards);
            reader.ReadValueSafe(out milestones);
        }
    }
}