using Actors;
using UnityEngine;

namespace StatusEffects
{
    public abstract class StatusEffect
    {
        private readonly float duration;
        private float currentDurationTimer;

        protected StatusEffect(float duration)
        {
            this.duration = duration;
            currentDurationTimer = duration;
        }

        public bool IsActive => currentDurationTimer > 0f;
        public float NormalizedDuration => Mathf.Clamp01(currentDurationTimer / duration);
        public float RemainingDuration => Mathf.Clamp(currentDurationTimer, 0, duration);
        public virtual string DisplayName => string.Empty;

        public void UpdateDuration(float deltaTime) => currentDurationTimer -= deltaTime;
        public void ResetDuration() => currentDurationTimer = duration;
        
        public virtual void OnAdded(Character target) {}
        public virtual void OnUpdate(Character target) {}
        public virtual void OnRemoved(Character target) {}
    }
}