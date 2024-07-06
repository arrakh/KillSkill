using KillSkill.Characters;

namespace KillSkill.Battle
{
    public readonly struct BattleStartData
    {
        public readonly IEnemyData enemyData;
        public readonly string levelId;

        public BattleStartData(IEnemyData enemyData, string levelId)
        {
            this.enemyData = enemyData;
            this.levelId = levelId;
        }
    }
}