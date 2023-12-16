using System;
using System.Collections.Generic;
using System.Linq;
using Arr;
using CharacterResources;
using CharacterResources.Implementations;
using DefaultNamespace;
using Skills;
using StatusEffects;
using UnityEngine;
using UnityEngine.Events;
using VisualEffects;

namespace Actors
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private EffectController effectController; //TEMP, SHOULD BE INJECTED FROM OUTSIDE
        [SerializeField] private float hp, maxHp; //TEMP, SHOULD BE DATA DRIVEN
        [SerializeField] private CharacterAnimator animator;

        protected StatusEffectsHandler statusEffects;
        protected CharacterResourcesHandler resources;

        protected Skill[] skills;

        //HANDLERS, ABSTRACT EVERYTHING HERE AND INJECT WHAT IS NEEDED
        public StatusEffectsHandler StatusEffects => statusEffects;
        
        public CharacterResourcesHandler Resources => resources;

        public CharacterAnimator Animator => animator;
        
        public IVisualEffectsHandler VisualEffects => effectController;
        
        public PersistentEventTemplate<Character> onInitialize = new();
        //==============================================================

        public float CooldownMultiplier { get; private set; }

        public Timer GlobalCooldown => globalCd;

        private Timer globalCd = new(0, false);

        public virtual Type MainResource => typeof(Health);

        public void Initialize()
        {
            statusEffects = new(this);
            resources = new();
            
            resources.Assign(new Health(this, hp, maxHp));
            
            onInitialize.Invoke(this);
            
            animator.Initialize();
            
            animator.PlayFlipBook("idle");
        }


        public bool CanCastAbility(Skill skill)
        {
            bool canCast = !statusEffects.Has<IPreventAbilityCasting>();

            if (skill is IGlobalCooldownSkill) return canCast && !globalCd.IsActive;

            return canCast;
        }

        public void SetCooldownSpeed(float multiplier)
        {
            CooldownMultiplier = multiplier;
            globalCd.SetSpeed(multiplier);
            foreach (var skill in skills)
                skill.Cooldown.SetSpeed(multiplier);
        }

        private void Update()
        {
            globalCd.Update(Time.deltaTime);
            statusEffects.Update(Time.deltaTime);
            
            foreach (var skill in skills)
                skill.UpdateCooldown(Time.deltaTime);
            
            OnUpdate();
        }
        
        protected virtual void OnUpdate(){}

        public Skill GetSkill(int index) => skills[index];

        public bool TryGetSkill(int index, out Skill skill)
        {
            skill = default;
            if (index >= skills.Length) return false;
            skill = skills[index];
            return true;
        }

        protected void ExecuteSkill(int index, Character target)
        {
            if (index >= skills.Length)
                throw new Exception($"Trying to execute Skill index {index} but there are only {skills.Length} skills!");
            
            var skill = skills[index];
            if (!skill.CanExecute(this)) return;
            
            skill.Execute(this, target);
            skill.TriggerCooldown();
            
            if (skill is IGlobalCooldownSkill) globalCd.Set(skill.Cooldown.Duration);
        }

        public void Kill()
        {
            
        }
    }
}