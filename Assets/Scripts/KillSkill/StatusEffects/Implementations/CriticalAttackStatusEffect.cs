using System;
using KillSkill.Characters;
using KillSkill.Database;
using KillSkill.Skills;
using KillSkill.StatusEffects.Implementations.Core;
using StatusEffects;
using UnityEngine;

namespace KillSkill.StatusEffects.Implementations
{
    public class CriticalAttackStatusEffect : TimedStatusEffect, IModifyDamageDealt
    {
        private float successChance, multiplier;

        public CriticalAttackStatusEffect(float duration, float successChance, float multiplier) : base(duration)
        {
            this.successChance = successChance;
            this.multiplier = multiplier;
        }

        public override StatusEffectDescription Description => new()
        {
            icon = SpriteDatabase.Get("status-critical-attack"),
            name = "Critical Attack",
            description = StandardDescription(successChance, multiplier)
        };
        
        public static string StandardDescription(float successChance, float multiplier) 
            => $"When attacking, there is a {successChance}% damage will be multiplied by x{multiplier:F2}";
        
        public void ModifyDamage(ICharacter damager, ICharacter target, ref double damage)
        {
            var success = UnityEngine.Random.Range(0f, 100f) < successChance;
            if (!success) return;

            damage *= multiplier;
            target.ShowFlyingText("Critical", Color.yellow, Vector3.up);
        }
        
        protected override void OnDuplicateAdded(ICharacter target, IStatusEffect duplicate)
        {
            base.OnDuplicateAdded(target, duplicate);

            if (duplicate is not CriticalAttackStatusEffect crit) throw new Exception("Duplicate is not Critical Attack?!");

            multiplier = crit.multiplier;
            successChance = crit.successChance;
        }
    }
}