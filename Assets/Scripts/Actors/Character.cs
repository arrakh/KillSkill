using System;
using System.Collections.Generic;
using System.Linq;
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

        public event Action<float, float> OnHealthChanged;

        protected Skill[] skills;
        protected Dictionary<Type, StatusEffect> statusEffects = new();

        public float Hp => hp;
        public float MaxHp => maxHp;
        public CharacterAnimator Animator => animator;
        
        public float CooldownMultiplier { get; private set; }

        public Timer GlobalCooldown => globalCd;

        private Timer globalCd = new(0, false);

        public void Initialize()
        {
            
        }

        public float VisualDistance(Character other) =>
            Vector3.Distance(animator.Visual.position, other.Animator.Visual.position);

        public void Damage(Character damager, float damage)
        {
            foreach (var statusEffect in statusEffects.Values)
                if(statusEffect is IModifyDamageStatusEffect modifier)
                    modifier.ModifyDamage(damager, ref damage);
            
            var oldHp = hp;
            hp -= damage;
            Debug.Log($"{name} Damaged for {damage}!");
            OnHealthChanged?.Invoke(oldHp, hp);

            var intensityAlpha = damage / maxHp;
            var remapped = Mathf.Lerp(0.5f, 3f, intensityAlpha);
            if (damage > 0f) animator.Damage(remapped);
        }

        public bool CanCastAbility(Skill skill)
        {
            bool canCast = !HasStatusEffect<IPreventAbilityCasting>();

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


        public void Heal(Character healer, float heal)
        {
            foreach (var statusEffect in statusEffects.Values)
                if(statusEffect is IModifyHealStatusEffect modifier)
                    modifier.ModifyDamage(healer, ref heal);
            
            var oldHp = hp;
            hp += heal;
            Debug.Log($"{name} Healed for {heal}!");
            OnHealthChanged?.Invoke(oldHp, hp);
        }

        private void Update()
        {
            globalCd.Update(Time.deltaTime);
            
            foreach (var statusEffect in statusEffects.Values.ToList())
            {
                statusEffect.UpdateDuration(Time.deltaTime);
                if (statusEffect.IsActive) statusEffect.OnUpdate(this);
                else
                {
                    statusEffect.OnRemoved(this);
                    statusEffects.Remove(statusEffect.GetType());
                }
            }
            
            foreach (var skill in skills)
                skill.UpdateCooldown(Time.deltaTime);
            
            OnUpdate();
        }
        
        protected virtual void OnUpdate(){}

        public void AddStatusEffect(StatusEffect statusEffect)
        {
            var type = statusEffect.GetType();
            if (statusEffects.TryGetValue(type, out var effect))
            {
                effect.OnDuplicateAdded(this);
                return;
            }
            
            statusEffects[type] = statusEffect;
            Debug.Log($"ADDING STATUS EFFECT {statusEffect.DisplayName}, has prevent? {HasStatusEffect<IPreventAbilityCasting>()}");

            statusEffects[type].OnAdded(this);
            
        }

        public void RemoveStatusEffect<T>() where T : StatusEffect
        {
            statusEffects[typeof(T)].OnRemoved(this);
            statusEffects.Remove(typeof(T));
        }

        public void RemoveStatusEffects<T>()
        {
            foreach (var statusEffect in statusEffects)
            {
                if (statusEffect.Key is T)
                {
                    statusEffects[statusEffect.Key].OnRemoved(this);
                    statusEffects.Remove(statusEffect.Key);
                }
            }
        }

        public bool HasStatusEffect<T>()
        {
            foreach (var stats in statusEffects.Values)
                if (stats is T) return true;

            return false;
        }

        public IEnumerable<StatusEffect> GetStatusEffects() => statusEffects.Values;

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
    }
}