using System;
using KillSkill.Characters;
using KillSkill.Database;
using KillSkill.Skills;
using KillSkill.StatusEffects.Implementations.Core;
using StatusEffects;

namespace KillSkill.StatusEffects.Implementations
{
    public class EmpowerStatusEffect : TimedStatusEffect, IModifyDamageDealt
    {
        private float multiplier;
        
        public EmpowerStatusEffect(float multiplier, float duration) : base(duration)
        {
            this.multiplier = multiplier;
        }

        public override StatusEffectDescription Description => new()
        {
            icon = SpriteDatabase.Get("status-empower"),
            name = "Empower",
            description = StandardDescription(multiplier)
        };

        public static string StandardDescription() => "While active, the user's attacks is more powerful";
        public static string StandardDescription(float multiplier) => $"While active, the user's attacks is x{multiplier:F2} more powerful";
        
        protected override void OnDuplicateAdded(Character target, IStatusEffect duplicate)
        {
            if (duplicate is not EmpowerStatusEffect empower) throw new Exception("Duplicate is not Open Wide?!");

            if (empower.multiplier > multiplier) multiplier = empower.multiplier;
            
            base.OnDuplicateAdded(target, duplicate);
        }
        
        public void ModifyDamage(Character damager, Character target, ref double damage)
        {
            damage *= multiplier;
        }
    }
}