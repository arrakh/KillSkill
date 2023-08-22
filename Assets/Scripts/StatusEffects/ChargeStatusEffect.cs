using System;
using Actors;

namespace StatusEffects
{
    public class ChargeStatusEffect : StatusEffect
    {
        public override string DisplayName => "Charging";
    
        private Action onDoneCharging;
        private Action onChargingInterrupted;

        public ChargeStatusEffect(float duration, Action onDoneCharging, Action onChargingInterrupted) : base(duration)
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