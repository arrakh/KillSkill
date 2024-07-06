using System;
using Arr;
using KillSkill.CharacterResources;
using KillSkill.CharacterResources.Implementations;
using KillSkill.Skills;
using StatusEffects;
using UnityEngine;
using VisualEffects;

namespace KillSkill.Characters
{
    public class Character : MonoBehaviour, ICharacter
    {
        [SerializeField] protected float hp, maxHp; //todo: TEMP, SHOULD BE DATA DRIVEN
        [SerializeField] private CharacterAnimator animator;
        [SerializeField] protected bool battlePause = false;
        [SerializeField] protected Character target;
        
        protected StatusEffectsHandler statusEffects;
        protected CharacterResourcesHandler resources;
        protected CharacterSkillHandler skillHandler;

        private ICharacterFactory characterFactory;
        private IVisualEffectsHandler visualEffects;
        private PersistentEventTemplate<ICharacter> onInitialize = new();

        protected bool isAlive = true;

        public bool IsAlive => isAlive;

        public int Uid => gameObject.GetInstanceID();

        public event Action<ICharacter> onDeath;
        
        public ICharacter Target { get; private set; }
        
        public IStatusEffectsHandler StatusEffects => statusEffects;
        
        public ICharacterResourcesHandler Resources => resources;

        public ICharacterAnimator Animator => animator;
        public ICharacterSkillHandler Skills => skillHandler;
        
        public IVisualEffectsHandler VisualEffects => visualEffects;
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

        public virtual void Initialize(ICharacterData characterData, Skill[] skills, ICharacterFactory factory, IVisualEffectsHandler vfx)
        {
            Target = target;
            isAlive = true;
            statusEffects = new(this);
            resources = new();
            skillHandler = new CharacterSkillHandler(skills, statusEffects, this);
            characterFactory = factory;
            visualEffects = vfx;

            hp = maxHp = characterData.Health;
            resources.Assign(new Health(this, hp, maxHp));
            
            skillHandler.InitializeSkills();
            
            animator.Initialize(characterData);
            
            animator.PlayFlipBook("idle");
            
            onInitialize.Invoke(this);
        }

        public void SetBattlePaused(bool paused) => battlePause = paused;

        public void SetTarget(ICharacter newTarget) => Target = newTarget;

        private void Update()
        {
            if (battlePause || !isAlive) return;
            var time = Time.deltaTime;
            statusEffects.Update(time);
            skillHandler.Update(time);
            resources.Update(time);
            
            OnUpdate();
        }
        
        protected virtual void OnUpdate(){}

        public void Kill()
        {
            isAlive = false;
            Debug.Log("KILL");

            if (animator.HasFlipBook("death")) animator.PlayFlipBook("death", 1f, null, false);
            
            OnDeathInternal();
        }

        private void OnDeathInternal()
        {
            Debug.Log("DEATH INTERNAL");
            OnDeath();
            onDeath?.Invoke(this);
        }

        protected virtual void OnDeath(){}
    }
}