using CleverCrow.Fluid.BTs.Trees;
using KillSkill.Network;
using KillSkill.Skills;
using Unity.Netcode;
using UnityEngine;
using VisualEffects;

namespace KillSkill.Characters
{
    public class NpcCharacter : Character
    {
        [SerializeField] private BehaviorTree aiTree;

        private INpcDefinition npcDefinition;

        private bool treeInitialized = false;
        
        public void ServerInitialize(uint characterId, bool isEnemy, INpcDefinition definition, ICharacterFactory factory)
        {
            var characterData = new CharacterData(definition.Id, definition.Health, definition.SkillTypes);

            npcDefinition = definition;
            
            base.ServerInitialize(characterId, isEnemy, characterData, factory);
            
        }

        protected override void OnClientInitialized()
        {
            base.OnClientInitialized();
            var builder = npcDefinition.OnBuildBehaviourTree(this, new BehaviorTreeBuilder(gameObject));
            aiTree = builder.Build();
            Debug.Log("NPC INITIALIZED");
            treeInitialized = true;
        }


        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (!IsServer) return;
            if (!treeInitialized) return;
            aiTree.Tick();
        }

        protected override void OnDeath()
        {
            if (!Net.IsServer()) return;
            GetComponent<NetworkObject>().Despawn();
            Destroy(gameObject);
        }
    }
}