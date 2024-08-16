using CleverCrow.Fluid.BTs.Trees;
using KillSkill.Skills;
using UnityEngine;
using VisualEffects;

namespace KillSkill.Characters
{
    public class NpcCharacter : Character
    {
        [SerializeField] private BehaviorTree aiTree;

        public void ServerInitialize(INpcDefinition definition, ICharacterFactory factory)
        {
            var characterData = new CharacterData(definition.Id, definition.Health, definition.SkillTypes);
            
            var builder = definition.OnBuildBehaviourTree(this, new BehaviorTreeBuilder(gameObject));
            aiTree = builder.Build();
            
            ServerInitialize(characterData, factory);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (!IsServer) return;
            if (battlePause) return;
            aiTree.Tick();
        }

        protected override void OnDeath()
        {
            Destroy(gameObject);
        }
    }
}