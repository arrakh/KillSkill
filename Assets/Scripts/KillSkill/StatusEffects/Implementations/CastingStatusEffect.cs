using System;
using Database;
using KillSkill.Characters;
using KillSkill.StatusEffects;
using KillSkill.StatusEffects.Implementations.Core;

namespace StatusEffects
{
    public class CastingStatusEffect : TimerStatusEffect
    {
        public override StatusEffectDescription Description => new()
        {
            icon = SpriteDatabase.Get("status-casting"),
            name = "Casting",
            description = "This character is casting something..."
        };

        private Action onDoneCharging;
        private Action onChargingInterrupted;

        public CastingStatusEffect(float duration, Action onDoneCharging, Action onChargingInterrupted) : base(duration)
        {
            this.onDoneCharging = onDoneCharging;
            this.onChargingInterrupted = onChargingInterrupted;
        }

        public override void OnRemoved(Character target)
        {
            if (timer.IsActive) onChargingInterrupted?.Invoke();
            else onDoneCharging?.Invoke();
        }
    }
}