using System;
using KillSkill.Characters;
using KillSkill.Database;
using KillSkill.Skills;
using KillSkill.StatusEffects.Implementations.Core;
using StatusEffects;

namespace KillSkill.StatusEffects.Implementations
{
    public class EmpoweredStatusEffect : TimedStatusEffect, IModifyDamageDealt
    {
        private float multiplier;
        
        public EmpoweredStatusEffect(float multiplier, float duration) : base(duration)
        {
            this.multiplier = multiplier;
        }

        public override StatusEffectDescription Description => new()
        {
            icon = SpriteDatabase.Get("status-empowered"),
            name = "Empowered",
            description = StandardDescription(multiplier)
        };

        public static string StandardDescription() => "While active, the user's attacks is more powerful";
        public static string StandardDescription(float multiplier) => $"While active, the user's attacks is x{multiplier:F2} more powerful";
        
        protected override void OnDuplicateAdded(ICharacter target, IStatusEffect duplicate)
        {
            base.OnDuplicateAdded(target, duplicate);

            if (duplicate is not EmpoweredStatusEffect empower) throw new Exception("Duplicate is not Empowered?!");

            if (empower.multiplier > multiplier) multiplier = empower.multiplier;
        }
        
        public void ModifyDamage(ICharacter damager, ICharacter target, ref double damage)
        {
            damage *= multiplier;
        }
    }
}