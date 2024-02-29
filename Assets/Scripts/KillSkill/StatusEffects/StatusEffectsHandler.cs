using System;
using System.Collections.Generic;
using System.Linq;
using Actors;
using DG.Tweening;
using UnityEngine;

namespace StatusEffects
{
    public class StatusEffectsHandler : IStatusEffectsHandler
    {

        public event StatusEffectEvent OnAdded;
        public event StatusEffectEvent OnRemoved;
        public event StatusEffectEvent OnUpdated;
        
        private Dictionary<Type, StatusEffect> statusEffects = new();
        private Character character;

        public StatusEffectsHandler(Character character)
        {
            this.character = character;
        }

        public void Update(float deltaTime)
        {
            foreach (var statusEffect in statusEffects.Values.ToList())
            {
                statusEffect.UpdateDuration(deltaTime);
                if (statusEffect.IsActive)
                {
                    statusEffect.OnUpdate(character);
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
        
        public void Add(StatusEffect statusEffect)
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

        public void Remove<T>() where T : StatusEffect
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

        public IEnumerable<StatusEffect> GetAll() => statusEffects.Values;
    }
}