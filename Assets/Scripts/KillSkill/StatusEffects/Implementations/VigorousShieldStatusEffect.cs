﻿using Database;
using KillSkill.CharacterResources.Implementations;
using KillSkill.Characters;
using KillSkill.Skills;
using KillSkill.StatusEffects.Implementations.Core;
using StatusEffects;

namespace KillSkill.StatusEffects.Implementations
{
    public class VigorousShieldStatusEffect : TimerStatusEffect, IModifyHealingDealt
    {
        private float shieldConversionPercent;

        public VigorousShieldStatusEffect(float shieldConversionPercent, float duration) : base(duration)
        {
            this.shieldConversionPercent = shieldConversionPercent;
        }

        public override StatusEffectDescription Description => new()
        {
            icon = SpriteDatabase.Get("skill-vigorous-shield"),
            name = "Vigorous Shield",
            description = $"When healing, gain {shieldConversionPercent}% of your shield as extra heal"
        };


        public void ModifyHeal(Character damager, Character target, ref double damage)
        {
            if (!damager.Resources.TryGet(out Shield shield)) return;

            if (shield.Charge < 0) return;

            var extraDamage = shield.Charge * (shieldConversionPercent / 100f);

            damage += extraDamage;
        }
    }
}