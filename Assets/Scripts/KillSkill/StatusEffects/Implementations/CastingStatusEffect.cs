using System;
using KillSkill.Characters;
using KillSkill.Database;
using KillSkill.Skills;
using KillSkill.StatusEffects;
using KillSkill.StatusEffects.Implementations;
using KillSkill.StatusEffects.Implementations.Core;

namespace StatusEffects
{
    public class CastingStatusEffect : TimedStatusEffect, IModifyIncomingDamage
    {
        public override StatusEffectDescription Description => new()
        {
            icon = SpriteDatabase.Get("status-casting"),
            name = "Casting",
            description = "This character is casting something..."
        };

        private Action onDoneCharging;

        public CastingStatusEffect(float duration, Action onDoneCharging) : base(duration)
        {
            this.onDoneCharging = onDoneCharging;
        }

        public override void OnRemoved(ICharacter target)
        {
            onDoneCharging?.Invoke();
        }

        public void ModifyDamage(ICharacter damager, ICharacter target, ref double damage)
        {
            
        }
    }
}