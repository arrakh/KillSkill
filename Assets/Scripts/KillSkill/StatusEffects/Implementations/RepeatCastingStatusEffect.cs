using System;
using KillSkill.Characters;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations.Core;
using StatusEffects;

namespace KillSkill.StatusEffects.Implementations
{
    public class RepeatCastingStatusEffect : TimedStatusEffect
    {
        public override StatusEffectDescription Description => new()
        {
            icon = SpriteDatabase.Get("status-casting"),
            name = "Casting",
            description = "This character is casting something..."
        };

        private Action<int> onDoneCycle;
        private int repeatCount;
        private int maxRepeat;

        public RepeatCastingStatusEffect(float duration, int repeatCount, Action<int> onDoneCycle) : base(duration)
        {
            this.onDoneCycle = onDoneCycle;
            maxRepeat = this.repeatCount = repeatCount;
        }

        protected override void OnUpdateDuration(float deltaTime)
        {
            base.OnUpdateDuration(deltaTime);
            if (timer.IsActive) return;
            repeatCount--;
            onDoneCycle?.Invoke(maxRepeat - repeatCount);
            
            if (repeatCount > 0) timer.Reset();
        }
    }
}