using System.Collections.Generic;

namespace KillSkill.Battle
{
    public readonly struct BattleResultData
    {
        public readonly bool playerWon;
        public readonly IEnumerable<BattleReward> rewards;

        public BattleResultData(bool playerWon, IEnumerable<BattleReward> rewards)
        {
            this.playerWon = playerWon;
            this.rewards = rewards;
        }
    }
}