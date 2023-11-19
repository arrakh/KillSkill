using System;
using System.Collections.Generic;
using System.Linq;
using CharacterResources;
using DefaultNamespace;
using Skills;
using StatusEffects;
using UnityEngine;
using UnityEngine.Events;

namespace Actors
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private float hp, maxHp;
        [SerializeField] private CharacterAnimator animator;

        protected StatusEffectsHandler statusEffects;
        protected CharacterResourcesHandler resources;

        public StatusEffectsHandler StatusEffects => statusEffects;
        public CharacterResourcesHandler Resources => resources;

        protected Skill[] skills;
        public CharacterAnimator Animator => animator;
        
        public float CooldownMultiplier { get; private set; }

        public Timer GlobalCooldown => globalCd;

        private Timer globalCd = new(0, false);

        public void Initialize()
        {
            statusEffects = new(this);
            
            
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