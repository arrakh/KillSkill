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
        [SerializeField] private EffectController effectController; //todo: TEMP, SHOULD BE INJECTED FROM OUTSIDE
        [SerializeField] private float hp, maxHp; //todo: TEMP, SHOULD BE DATA DRIVEN
        [SerializeField] private CharacterAnimator animator;
        [SerializeField] protected bool battlePause = true;
        [SerializeField] protected Character target;
        
        protected StatusEffectsHandler statusEffects;
        protected CharacterResourcesHandler resources;

        
        protected Skill[] skills;
        protected Dictionary<Type, int> skillIndexes = new();

        protected bool isAlive = true;

        public bool IsAlive => isAlive;

        public event Action onDeath;

        //todo: HANDLERS, ABSTRACT EVERYTHING HERE AND INJECT WHAT IS NEEDED
        public Character Target => target;
        
        public IStatusEffectsHandler StatusEffects => statusEffects;
        
        public CharacterResourcesHandler Resources => resources;

        public CharacterAnimator Animator => animator;
        
        public IVisualEffectsHandler VisualEffects => effectController;
        
        public PersistentEventTemplate<Character> onInitialize = new();
        //==============================================================

        public float CooldownMultiplier { get; private set; }

        public Timer GlobalCooldown => globalCd;

        private Timer globalCd = new(0, false);

        public virtual Type MainResource => typeof(Health);

        public void Initialize(ICharacterData characterData)
        {
            isAlive = true;
            statusEffects = new(this);
            resources = new();
            
            resources.Assign(new Health(this, hp, maxHp));
            
            onInitialize.Invoke(this);
            
            animator.Initialize(characterData);
            
            animator.PlayFlipBook("idle");

            for (var index = 0; index < skills.Length; index++)
            {
                var skill = skills[index];
                if (skill == null) continue;
                skillIndexes[skill.GetType()] = index;
            }
        }

        public void SetBattlePaused(bool paused) => battlePause = paused;

        public bool CanCastAbility(Skill skill)
        {
            if (!isAlive) return false;
            
            bool canCast = !statusEffects.Has<IPreventCasting>();

            if (skill is IGlobalCooldownSkill) return canCast && !globalCd.IsActive;

            return canCast;
        }

        public bool CanCastAbility(int index)
        {
            if (index < 0 || index >= skills.Length) return false;
            var skill = skills[index];
            if (skill == null) return false;
            return CanCastAbility(skill);
        }

        public void SetCooldownSpeed(float multiplier)
        {
            CooldownMultiplier = multiplier;
            globalCd.SetSpeed(multiplier);
            foreach (var skill in skills)
                skill?.Cooldown.SetSpeed(multiplier);
        }

        private void Update()
        {
            if (battlePause || !isAlive) return;
            globalCd.Update(Time.deltaTime);
            statusEffects.Update(Time.deltaTime);
            
            foreach (var skill in skills)
                skill?.UpdateCooldown(Time.deltaTime);
            
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

        public bool TryGetSkillIndex<T>(out int skillIndex)
            => skillIndexes.TryGetValue(typeof(T), out skillIndex);

        public void ExecuteSkill<T>(Character target) where T : Skill
        {
            if (!skillIndexes.TryGetValue(typeof(T), out var index))
                throw new Exception($"Trying to execute {typeof(T)} but character does not have it!");
            
            ExecuteSkill(index, target);
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
            isAlive = false;
            Debug.Log("KILL");

            if (animator.HasFlipBook("death")) animator.PlayFlipBook("death", 1f, null, false);
            
            OnDeathInternal();
        }

        private void OnDeathInternal()
        {
            Debug.Log("DEATH INTERNAL");
            OnDeath();
            onDeath?.Invoke();
        }

        protected virtual void OnDeath(){}
    }
}