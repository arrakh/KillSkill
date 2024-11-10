using KillSkill.Characters;
using KillSkill.Database;
using KillSkill.Skills;
using KillSkill.StatusEffects.Implementations.Core;
using StatusEffects;
using VisualEffects;

namespace KillSkill.StatusEffects.Implementations
{
    public class FortifiedStatusEffect : TimedStatusEffect, IStatusEffect, IModifyIncomingDamage
    {
        private float multiplier;
        private IEffect shieldEffect;

        public FortifiedStatusEffect(float duration, float multiplier) : base(duration)
        {
            this.multiplier = multiplier;
        }

        public override StatusEffectDescription Description => new()
        {
            icon = SpriteDatabase.Get("status-empowered"),
            name = "Empowered",
            description = StandardDescription(multiplier)
        };
        
        public static string StandardDescription(float percent) => $"Incoming damage is reduced by {percent:F0}%";

        public void ModifyDamage(ICharacter damager, ICharacter target, ref double damage)
        {
            damage *= multiplier;
        }

        protected override void OnDuplicateAdded(ICharacter target, IStatusEffect duplicate)
        {
            base.OnDuplicateAdded(target, duplicate);
            if (duplicate is not FortifiedStatusEffect dupeFortify) return;
            if (dupeFortify.multiplier < multiplier) multiplier = dupeFortify.multiplier;
        }

        public override void OnAdded(ICharacter target)
        {
            base.OnAdded(target);
            shieldEffect = target.VisualEffects.Spawn("shield-on", target.Position);
        }

        public override void OnRemoved(ICharacter target)
        {
            base.OnRemoved(target);
            shieldEffect?.Stop();
            target.VisualEffects.Spawn("shield-off", target.Position);
        }
    }
}