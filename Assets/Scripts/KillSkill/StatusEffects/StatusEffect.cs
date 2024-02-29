using Actors;
using DefaultNamespace;
using UnityEngine;

namespace StatusEffects
{
    public abstract class StatusEffect
    {
        private readonly Timer timer;

        protected StatusEffect(float duration)
        {
            timer = new Timer(duration);
        }

        public bool IsActive => timer.IsActive;
        public float NormalizedDuration => timer.NormalizedTime;
        public float RemainingDuration => timer.RemainingTime;
        public virtual string DisplayName => string.Empty;

        public void UpdateDuration(float deltaTime) => timer.Update(deltaTime);

        public virtual void OnDuplicateAdded(Character target) => timer.Reset();
        
        public virtual void OnAdded(Character target) {}
        public virtual void OnUpdate(Character target) {}
        public virtual void OnRemoved(Character target) {}

        public virtual StatusEffectDescription Description => new();
    }
}