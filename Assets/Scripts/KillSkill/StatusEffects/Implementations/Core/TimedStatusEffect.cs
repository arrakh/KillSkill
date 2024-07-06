using KillSkill.Characters;
using StatusEffects;

namespace KillSkill.StatusEffects.Implementations.Core
{
    public abstract class TimedStatusEffect : IStatusEffect, ITimedStatusEffect
    {
        public float NormalizedDuration => timer.NormalizedTime;
        public float RemainingDuration => timer.RemainingTime;
        
        protected readonly Timer timer;

        protected TimedStatusEffect(float duration)
        {
            timer = new Timer(duration);
        }

        bool IStatusEffect.IsActive => timer.IsActive;
        public abstract StatusEffectDescription Description { get; }

        void IStatusEffect.OnDuplicateAdded(ICharacter target, IStatusEffect duplicate)
        {
            timer.Reset();
            OnDuplicateAdded(target, duplicate);
        }

        void ITimedStatusEffect.UpdateDuration(float deltaTime)
        {
            timer.Update(deltaTime);
            OnUpdateDuration(deltaTime);
        }

        protected virtual void OnUpdateDuration(float deltaTime){}
        protected virtual void OnDuplicateAdded(ICharacter target, IStatusEffect duplicate){}

        public virtual void OnAdded(ICharacter target){}

        public virtual void OnUpdate(ICharacter target, float deltaTime){}

        public virtual void OnRemoved(ICharacter target) {}
    }
}