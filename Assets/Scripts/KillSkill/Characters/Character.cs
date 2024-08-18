using System;
using Arr;
using Arr.EventsSystem;
using Arr.Utils;
using KillSkill.CharacterResources;
using KillSkill.CharacterResources.Implementations;
using KillSkill.Minions;
using KillSkill.Modules.Battle.Events;
using KillSkill.Modules.VisualEffects;
using KillSkill.Skills;
using KillSkill.StatusEffects;
using KillSkill.UI.Game;
using StatusEffects;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using VisualEffects;

namespace KillSkill.Characters
{
    public class Character : NetworkBehaviour, ICharacter
    {
        [SerializeField] private CharacterAnimator animator;
        [SerializeField] protected NetworkVariable<bool> battlePause = new ();
        [SerializeField] protected CharacterResourcesDisplay resourcesDisplay;

        [SerializeField] private StatusEffectsHandler statusEffects;
        [SerializeField] private CharacterResourcesHandler resources;
        [SerializeField] private CharacterSkillHandler skillHandler;
        [SerializeField] private CharacterMinionHandler minionsHandler;

        private ICharacterRegistry characterRegistry;
        private IVisualEffectsHandler visualEffects;
        private PersistentEventTemplate<ICharacter> onInitialize = new();
        private PersistentEventTemplate<ICharacter> onTargetUpdated = new();

        private uint characterId;

        private bool isEnemy = false;

        protected NetworkVariable<bool> isAlive = new();

        public bool IsAlive => isAlive.Value;

        public uint Id => characterId;

        public bool IsEnemy => isEnemy;

        public event Action<ICharacter> onDeath;
        
        public ICharacter Target { get; private set; }
        
        public IStatusEffectsHandler StatusEffects => statusEffects;
        
        public ICharacterResourcesHandler Resources => resources;

        public ICharacterAnimator Animator => animator;
        public ICharacterSkillHandler Skills => skillHandler;
        
        public IVisualEffectsHandler VisualEffects => visualEffects;
        public ICharacterMinionHandler Minions => minionsHandler;
        public ICharacterRegistry Registry => characterRegistry;
        
        public PersistentEventTemplate<ICharacter> OnInitialize => onInitialize;
        public PersistentEventTemplate<ICharacter> OnTargetUpdated => onTargetUpdated;

        public Vector3 Position
        {
            set
            {
                animator.UpdatePosition(value);
                transform.position = value;
            }

            get => transform.position;
        }

        public GameObject GameObject => gameObject;
        public virtual Type MainResource => typeof(Health);

        public void ServerInitialize(uint id, bool enemy, CharacterData characterData, ICharacterFactory factory)
        {
            minionsHandler.Initialize(factory, this);
            isAlive.Value = true;

            ClientInitializeRpc(id, enemy, characterData);
        }

        [Rpc(SendTo.Everyone)]
        private void ClientInitializeRpc(uint id, bool enemy, CharacterData characterData)
        {
            characterId = id;
            characterRegistry = ICharacterRegistry.GetHandle();
            isEnemy = enemy;

            characterRegistry.TryRegister(id, this);

            Debug.Log($"TEST CLIENT CHARACTER DATA ID {characterData.Id}");
            
            visualEffects = Vfx.GetHandler();

            statusEffects.Initialize(this);

            skillHandler.Initialize(characterData.SkillTypes, this);
            
            resources.Assign(new Health(this, characterData.Health, characterData.Health));
            
            skillHandler.InitializeSkills();
            
            resourcesDisplay.Initialize(this);
            
            animator.Initialize(characterData);
            
            animator.PlayFlipBook("idle");
            
            onInitialize.Invoke(this);

            OnClientInitialized();
        }

        protected virtual void OnClientInitialized(){}

        public void SetBattlePaused(bool paused) => battlePause.Value = paused;

        public void SetTarget(ICharacter newTarget)
        {
            ServerSetTargetRPC(newTarget.Id);
        }

        [Rpc(SendTo.Server)]
        private void ServerSetTargetRPC(uint newTargetId)
        {
            ClientSetTargetRpc(newTargetId);
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void ClientSetTargetRpc(uint newTargetId)
        {
            if (!Registry.TryGet(newTargetId, out var newTarget)) return;
            Target = newTarget;
            onTargetUpdated?.Invoke(Target);
        }

        private void Update()
        {
            if (battlePause.Value || !IsAlive) return;
            
            var time = Time.deltaTime;
            statusEffects.UpdateHandler(time);
            skillHandler.UpdateHandler(time);
            resources.UpdateHandler(time);
            
            OnUpdate();
        }
        
        protected virtual void OnUpdate(){}

        public void Kill()
        {
            if (!IsOwner) return;
            KillRpc();
        }

        [Rpc(SendTo.Everyone)]
        private void KillRpc()
        {
            isAlive.Value = false;
            Debug.Log("KILL");

            if (animator.HasFlipBook("death")) animator.PlayFlipBook("death", 1f, null, false);
            
            OnDeathInternal();
        }

        private void OnDeathInternal()
        {
            Debug.Log("DEATH INTERNAL");
            onDeath?.Invoke(this);
            OnDeath();
        }

        protected virtual void OnDeath(){}
    }
}