using KillSkill.Characters;

namespace KillSkill.Battle
{
    public readonly struct BattleStartData
    {
        public readonly IEnemyData enemyData;

        public BattleStartData(IEnemyData enemyData)
        {
            this.enemyData = enemyData;
        }
    }
}