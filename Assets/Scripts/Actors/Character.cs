using System;
using System.Collections.Generic;
using System.Linq;
using Skills;
using StatusEffects;
using UnityEngine;
using UnityEngine.Events;

namespace Actors
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private float hp, maxHp;

        public event Action<float, float> OnHealthChanged;

        protected Skill[] skills;
        protected Dictionary<Type, StatusEffect> statusEffects = new();

        public float Hp => hp;
        public float MaxHp => maxHp;

        public void Damage(float damage)
        {
            if (HasStatusEffect<BlockStatusEffect>())
            {
                Debug.Log("BLOCKED!!");
                return;
            }
            
            var oldHp = hp;
            hp -= damage;
            Debug.Log($"{name} Damaged for {damage}!");
            OnHealthChanged?.Invoke(oldHp, hp);
        }

        public void Heal(float heal)
        {
            var oldHp = hp;
            hp += heal;
            Debug.Log($"{name} Healed for {heal}!");
            OnHealthChanged?.Invoke(oldHp, hp);
        }

        private void Update()
        {
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
            if (statusEffects.TryGetValue(type, out var effect)) effect.ResetDuration();
            else statusEffects[type] = statusEffect;
            
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

        public bool HasStatusEffect<T>() where T : StatusEffect => statusEffects.ContainsKey(typeof(T));
        
        public bool HasAnyStatusEffect<T>()
        {
            foreach (var stats in statusEffects.Keys)
            {
                foreach (var statusEffect in statusEffects)
                    if (statusEffect.Key is T) return true;
            }

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
        }
    }
}