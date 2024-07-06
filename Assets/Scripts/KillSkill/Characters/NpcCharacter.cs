using CleverCrow.Fluid.BTs.Trees;
using KillSkill.Skills;
using UnityEngine;
using VisualEffects;

namespace KillSkill.Characters
{
    public class NpcCharacter : Character
    {
        [SerializeField] private BehaviorTree aiTree;

        private bool hasAi = false;

        public override void Initialize(ICharacterData characterData, ICharacterFactory factory, IVisualEffectsHandler vfx)
        {
            base.Initialize(characterData, factory, vfx);
            if (characterData is not IBehaviourTreeData btData)
            {
                hasAi = false;
                return;
            }

            hasAi = true;
            var builder = btData.OnBuildBehaviourTree(this, new BehaviorTreeBuilder(gameObject));
            aiTree = builder.Build();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (battlePause) return;
            if (hasAi) aiTree.Tick();
        }
    }
}