using System;
using Actors;
using Database;

namespace StatusEffects
{
    public class CastingStatusEffect : StatusEffect, IPreventCasting
    {
        public override string DisplayName => "Casting";

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
            if (IsActive) onChargingInterrupted?.Invoke();
            else onDoneCharging?.Invoke();
        }
    }
}