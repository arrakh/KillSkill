using System.Collections.Generic;
using KillSkill.Battle;
using KillSkill.Characters;
using KillSkill.Characters.Implementations.EnemyData;
using UnityEngine.Rendering;

namespace KillSkill.SessionData.Implementations
{
    public class BattleSessionData : ISessionData
    {
        private BattleStartData battleStart = new (new Executioner());

        public void SetBattle(IEnemyData enemy)
        {
            battleStart = new BattleStartData(enemy);
        }

        public IEnemyData GetEnemy() => battleStart.enemyData;
        
        public void OnLoad()
        {
            
        }

        public void OnUnload()
        {
            
        }
    }
}