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
        [SerializeField] protected bool battlePause = false;
        [SerializeField] protected CharacterResourcesDisplay resourcesDisplay;

        [SerializeField] private StatusEffectsHandler statusEffects;
        [SerializeField] private CharacterResourcesHandler resources;
        [SerializeField] private CharacterSkillHandler skillHandler;
        [SerializeField] private CharacterMinionHandler minionsHandler;

        private ICharacterFactory characterFactory;
        private IVisualEffectsHandler visualEffects;
        private PersistentEventTemplate<ICharacter> onInitialize = new();

        protected NetworkVariable<bool> isAlive;

        public bool IsAlive => isAlive.Value;

        public int Uid => gameObject.GetInstanceID();

        public event Action<ICharacter> onDeath;
        
        public ICharacter Target { get; private set; }
        
        public IStatusEffectsHandler StatusEffects => statusEffects;
        
        public ICharacterResourcesHandler Resources => resources;

        public ICharacterAnimator Animator => animator;
        public ICharacterSkillHandler Skills => skillHandler;
        
        public IVisualEffectsHandler VisualEffects => visualEffects;
        public ICharacterMinionHandler Minions => minionsHandler;
        public ICharacterFactory CharacterFactory => characterFactory;
        
        public PersistentEventTemplate<ICharacter> OnInitialize => onInitialize;

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

        public void ServerInitialize(CharacterData characterData, ICharacterFactory factory)
        {
            characterFactory = factory;
            minionsHandler.Initialize(factory, this);
            
            ClientInitializeRpc(characterData);
        }

        [Rpc(SendTo.Everyone)]
        private void ClientInitializeRpc(CharacterData characterData)
        {
            Debug.Log($"TEST CLIENT CHARACTER DATA ID {characterData.Id}");
            
            visualEffects = Vfx.GetHandler();

            isAlive.Value = true;
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

        public void SetBattlePaused(bool paused) => battlePause = paused;

        public void SetTarget(ICharacter newTarget) => Target = newTarget;

        private void Update()
        {
            if (battlePause || !IsAlive) return;

            if (!IsOwner)
            {
                OnUpdate();
                return;
            }
            
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