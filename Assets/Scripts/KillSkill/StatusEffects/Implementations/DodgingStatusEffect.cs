using System;
using KillSkill.Characters;
using KillSkill.Database;
using KillSkill.Skills;
using KillSkill.StatusEffects.Implementations.Core;
using StatusEffects;
using Random = UnityEngine.Random;

namespace KillSkill.StatusEffects.Implementations
{
    public class DodgingStatusEffect : TimedStatusEffect, IModifyIncomingDamage
    {
        private float successChance, multiplier;

        public DodgingStatusEffect(float duration, float successChance, float multiplier) : base(duration)
        {
            this.successChance = successChance;
            this.multiplier = multiplier;
        }

        public override StatusEffectDescription Description => new()
        {
            icon = SpriteDatabase.Get("status-dodging"),
            name = "Dodging",
            description = StandardDescription(successChance, multiplier)
        };
        
        public static string StandardDescription(float successChance, float multiplier) 
            => $"When damaged, there is a {successChance}% incoming damage will be reduced by x{multiplier:F2}";
        
        protected override void OnDuplicateAdded(ICharacter target, IStatusEffect duplicate)
        {
            base.OnDuplicateAdded(target, duplicate);

            if (duplicate is not DodgingStatusEffect dodging) throw new Exception("Duplicate is not Dodging?!");

            multiplier = dodging.multiplier;
            successChance = dodging.successChance;
        }
        
        public void ModifyDamage(ICharacter damager, ICharacter target, ref double damage)
        {
            var success = Random.Range(0f, 100f) < successChance;
            if (!success) return;

            damage *= multiplier;
        }
    }
}