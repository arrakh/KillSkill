using System;
using System.Collections.Generic;
using Arr;
using CharacterResources;
using DefaultNamespace;
using KillSkill.CharacterResources;
using KillSkill.CharacterResources.Implementations;
using KillSkill.Skills;
using Skills;
using StatusEffects;
using UnityEngine;
using VisualEffects;

namespace KillSkill.Characters
{
    //todo: CHARACTER SHOULD BE INTERFACED TO ICHARACTER
    public class Character : MonoBehaviour
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

        protected bool isAlive = true;

        public bool IsAlive => isAlive;

        public int Uid => gameObject.GetInstanceID();

        public event Action<Character> onDeath;

        //todo: HANDLERS, ABSTRACT EVERYTHING HERE AND INJECT WHAT IS NEEDED
        public Character Target => target;
        
        public IStatusEffectsHandler StatusEffects => statusEffects;
        
        public CharacterResourcesHandler Resources => resources;

        public CharacterAnimator Animator => animator;
        public CharacterSkillHandler Skills => skillHandler;
        
        public IVisualEffectsHandler VisualEffects => visualEffects;
        public ICharacterFactory CharacterFactory => characterFactory;

        public PersistentEventTemplate<Character> onInitialize = new();

        public Vector3 Position
        {
            set
            {
                animator.UpdatePosition(value);
                transform.position = value;
            }

            get => transform.position;
        }
        //==============================================================
        
        public virtual Type MainResource => typeof(Health);

        public virtual void Initialize(ICharacterData characterData, Skill[] skills, ICharacterFactory factory, IVisualEffectsHandler vfx)
        {
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

        public void SetTarget(Character newTarget) => target = newTarget;

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