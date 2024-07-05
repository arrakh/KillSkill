using CleverCrow.Fluid.BTs.Trees;
using KillSkill.Skills;
using UnityEngine;
using VisualEffects;

namespace KillSkill.Characters
{
    public class EnemyCharacter : Character
    {
        [SerializeField] private BehaviorTree aiTree;

        public override void Initialize(ICharacterData characterData, Skill[] skills, ICharacterFactory factory, IVisualEffectsHandler vfx)
        {
            base.Initialize(characterData, skills, factory, vfx);
            if (characterData is not IEnemyData enemyData) return;
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