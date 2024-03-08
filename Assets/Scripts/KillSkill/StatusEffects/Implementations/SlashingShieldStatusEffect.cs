using KillSkill.CharacterResources.Implementations;
using KillSkill.Characters;
using KillSkill.Database;
using KillSkill.Skills;
using KillSkill.Skills.Implementations.Fighter;
using KillSkill.StatusEffects.Implementations.Core;
using StatusEffects;
using UnityEngine;

namespace KillSkill.StatusEffects.Implementations
{
    public class SlashingShieldStatusEffect : TimedStatusEffect, IModifyDamageDealt
    {
        private float shieldConversionPercent;

        public SlashingShieldStatusEffect(float shieldConversionPercent, float duration) : base(duration)
        {
            this.shieldConversionPercent = shieldConversionPercent;
        }

        public override StatusEffectDescription Description => new()
        {
            icon = SpriteDatabase.Get("skill-slashing-shield"),
            name = "Slashing Shield",
            description = $"When attacking, the user gains {shieldConversionPercent}% of their shield as extra damage"
        };


        public void ModifyDamage(Character damager, Character target, ref double damage)
        {
            Debug.Log($"[SSSE] TRYING TO GET SHIELD with target {target.GetType().Name}");
            if (!damager.Resources.TryGet(out Shield shield)) return;
            
            Debug.Log($"[SSSE] SHIELD HAS {shield.Charge} CHARGE");

            if (shield.Charge < 0) return;

            var extraDamage = shield.Charge * (shieldConversionPercent / 100f);
            
            Debug.Log($"[SSSE] WILL ADD {extraDamage} EXTRA DMG");

            damage += extraDamage;
        }
    }
}