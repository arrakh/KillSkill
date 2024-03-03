using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using KillSkill.Characters;
using KillSkill.StatusEffects;
using UnityEngine;

namespace StatusEffects
{
    public class StatusEffectsHandler : IStatusEffectsHandler
    {

        public event StatusEffectEvent OnAdded;
        public event StatusEffectEvent OnRemoved;
        public event StatusEffectEvent OnUpdated;
        
        private Dictionary<Type, IStatusEffect> statusEffects = new();
        private Character character;

        public StatusEffectsHandler(Character character)
        {
            this.character = character;
        }

        public void Update(float deltaTime)
        {
            foreach (var statusEffect in statusEffects.Values.ToList())
            {
                if (statusEffect is ITimerStatusEffect timer) timer.UpdateDuration(deltaTime);
                
                if (statusEffect.IsActive)
                {
                    statusEffect.OnUpdate(character, deltaTime);
                    OnUpdated?.Invoke(statusEffect);
                }
                else
                {
                    statusEffect.OnRemoved(character);
                    OnRemoved?.Invoke(statusEffect);
                    statusEffects.Remove(statusEffect.GetType());
                }
            }
        }
        
        public void Add(IStatusEffect statusEffect)
        {
            var type = statusEffect.GetType();
            if (statusEffects.TryGetValue(type, out var effect))
            {
                effect.OnDuplicateAdded(character);
                OnAdded?.Invoke(effect);
                return;
            }
            
            statusEffects[type] = statusEffect;

            statusEffects[type].OnAdded(character);
            OnAdded?.Invoke(statusEffect);
        }

        public void Remove<T>() where T : IStatusEffect
        {
            var toRemove = statusEffects[typeof(T)]; 
            toRemove.OnRemoved(character);
            OnRemoved?.Invoke(toRemove);
            statusEffects.Remove(typeof(T));
        }

        public void RemoveAny<T>()
        {
            foreach (var statusEffect in statusEffects)
            {
                if (statusEffect.Key is T)
                {
                    statusEffect.Value.OnRemoved(character);
                    OnRemoved?.Invoke(statusEffect.Value);
                    statusEffects.Remove(statusEffect.Key);
                }
            }
        }

        public bool Has<T>()
        {
            foreach (var stats in statusEffects.Values)
                if (stats is T) return true;

            return false;
        }

        public IEnumerable<IStatusEffect> GetAll() => statusEffects.Values;
    }
}