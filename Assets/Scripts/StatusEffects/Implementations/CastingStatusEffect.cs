using System;
using Actors;

namespace StatusEffects
{
    public class CastingStatusEffect : StatusEffect, IPreventAbilityCasting
    {
        public override string DisplayName => "Casting";
    
        private Action onDoneCharging;
        private Action onChargingInterrupted;

        public CastingStatusEffect(float duration, Action onDoneCharging, Action onChargingInterrupted) : base(duration)
        {
            this.onDoneCharging = onDoneCharging;
            this.onChargingInterrupted = onChargingInterrupted;
        }

        public override void OnRemoved(Character target)
        {
            if (IsActive) onChargingInterrupted?.Invoke();
            else onDoneCharging?.Invoke();
        }
    }
}