using System;
using KillSkill.Characters;
using KillSkill.Database;
using KillSkill.Skills;
using KillSkill.StatusEffects.Implementations.Core;
using StatusEffects;

namespace KillSkill.StatusEffects.Implementations
{
    public class OpenWideStatusEffect : TimedStatusEffect, IModifyIncomingDamage
    {
        private float multiplier;

        public OpenWideStatusEffect(float multiplier, float duration) : base(duration)
        {
            this.multiplier = multiplier;
        }

        public override StatusEffectDescription Description => new()
        {
            icon = SpriteDatabase.Get("status-open-wide"),
            name = "Open Wide",
            description = StandardDescription(multiplier)
        };

        public static string StandardDescription(float multiplier) => $"While active, the user receives x{multiplier:F1} more damage";
        public static string StandardDescription() => "While active, the user receives more damage";

        protected override void OnDuplicateAdded(Character target, IStatusEffect duplicate)
        {
            if (duplicate is not OpenWideStatusEffect openWide) throw new Exception("Duplicate is not Open Wide?!");

            if (openWide.multiplier > multiplier) multiplier = openWide.multiplier;
            
            base.OnDuplicateAdded(target, duplicate);
        }
        
        public void ModifyDamage(Character damager, Character target, ref double damage)
        {
            damage *= multiplier;
        }
    }
}