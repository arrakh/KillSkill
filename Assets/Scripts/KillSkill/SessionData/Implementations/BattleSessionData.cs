using System.Collections.Generic;
using KillSkill.Battle;
using KillSkill.Characters;
using KillSkill.Characters.Implementations.EnemyData;
using UnityEngine.Rendering;

namespace KillSkill.SessionData.Implementations
{
    public class BattleSessionData : ISessionData
    {
        private INpcDefinition currentEnemy = new Executioner();
        private string currentLevelId = "forest";
        
        public void SetBattle(INpcDefinition enemy) => currentEnemy = enemy;
        public void SetLevel(string levelId) => currentLevelId = levelId;

        public BattleStartData StartData => new (currentEnemy, currentLevelId);
    }
}