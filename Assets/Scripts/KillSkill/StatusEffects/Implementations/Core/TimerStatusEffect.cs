using KillSkill.Characters;
using StatusEffects;

namespace KillSkill.StatusEffects.Implementations.Core
{
    public abstract class TimerStatusEffect : IStatusEffect, ITimerStatusEffect
    {
        public float NormalizedDuration => timer.NormalizedTime;
        public float RemainingDuration => timer.RemainingTime;
        
        protected readonly Timer timer;

        protected TimerStatusEffect(float duration)
        {
            timer = new Timer(duration);
        }

        bool IStatusEffect.IsActive => timer.IsActive;
        public abstract StatusEffectDescription Description { get; }

        void IStatusEffect.OnDuplicateAdded(Character target)
        {
            timer.Reset();
            OnDuplicateAdded(target);
        }

        void ITimerStatusEffect.UpdateDuration(float deltaTime)
        {
            timer.Update(deltaTime);
            UpdateDuration(deltaTime);
        }

        protected virtual void UpdateDuration(float deltaTime){}
        protected virtual void OnDuplicateAdded(Character target){}

        public virtual void OnAdded(Character target){}

        public virtual void OnUpdate(Character target, float deltaTime){}

        public virtual void OnRemoved(Character target) {}
    }
}