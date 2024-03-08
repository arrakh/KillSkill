using CleverCrow.Fluid.BTs.Trees;
using KillSkill.Characters.Implementations.EnemyData;
using KillSkill.SessionData;
using KillSkill.SessionData.Implementations;
using KillSkill.Utility.BehaviourTree;
using Skills;
using UnityEngine;

namespace KillSkill.Characters
{
    public class EnemyCharacter : Character
    {
        [SerializeField] private BehaviorTree aiTree;

        private void Start()
        {
            var battleSession = Session.GetData<BattleSessionData>();
            var enemyData = battleSession.GetEnemy();
            
            Initialize(enemyData, enemyData.Skills);

            var builder = enemyData.OnBuildBehaviourTree(this, new BehaviorTreeBuilder(gameObject));
            aiTree = builder.Build();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (battlePause) return;
            aiTree.Tick();
        }
    }
}