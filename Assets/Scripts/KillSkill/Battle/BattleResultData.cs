using System.Collections.Generic;

namespace KillSkill.Battle
{
    public readonly struct BattleResultData
    {
        public readonly bool playerWon;
        public readonly Dictionary<string, double> gainedResources;

        public BattleResultData(bool playerWon, Dictionary<string, double> gainedResources)
        {
            this.playerWon = playerWon;
            this.gainedResources = gainedResources;
        }
    }
}