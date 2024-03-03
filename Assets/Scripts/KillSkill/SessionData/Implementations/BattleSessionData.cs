using System.Collections.Generic;
using KillSkill.Battle;
using KillSkill.Characters;
using KillSkill.Characters.Implementations.EnemyData;

namespace KillSkill.SessionData.Implementations
{
    public class BattleSessionData : ISessionData
    {
        private BattleStartData battleStart = new (new MushroomMan());

        public void SetBattle(IEnemyData enemy)
        {
            battleStart = new BattleStartData(enemy);
        }

        public IEnemyData GetEnemy() => battleStart.enemyData;

        public Dictionary<string, double> CalculateReward(BattleResultState state)
        {
            var rewards = new Dictionary<string, double>();

            foreach (var reward in battleStart.enemyData.Rewards)
            {
                var amount = reward.CalculateReward(state);
                if (amount == 0) continue;
                rewards[reward.Id] = amount;
            }

            return rewards;
        }
        
        public void OnLoad()
        {
            
        }

        public void OnUnload()
        {
            
        }
    }
}