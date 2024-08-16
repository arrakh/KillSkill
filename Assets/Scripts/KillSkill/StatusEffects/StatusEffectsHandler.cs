using System;
using System.Collections.Generic;
using System.Linq;
using KillSkill.Characters;
using StatusEffects;
using Unity.Netcode;

namespace KillSkill.StatusEffects
{
    public class StatusEffectsHandler : NetworkBehaviour, IStatusEffectsHandler
    {

        public event StatusEffectEvent OnAdded;
        public event StatusEffectEvent OnRemoved;
        public event StatusEffectEvent OnUpdated;
        
        private Dictionary<Type, IStatusEffect> statusEffects = new();
        private ICharacter character;

        public void Initialize(ICharacter owner)
        {
            this.character = owner;
        }

        public void UpdateHandler(float deltaTime)
        {
            foreach (var statusEffect in statusEffects.Values.ToList())
            {
                if (statusEffect is ITimedStatusEffect timer) timer.UpdateDuration(deltaTime);
                
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
                effect.OnDuplicateAdded(character, statusEffect);
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

        public bool TryRemove<T>() where T : IStatusEffect
        {
            if (!statusEffects.ContainsKey(typeof(T))) return false;
            Remove<T>();
            return true;
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