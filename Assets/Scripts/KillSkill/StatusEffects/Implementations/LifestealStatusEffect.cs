using KillSkill.CharacterResources.Implementations;
using KillSkill.Characters;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations.Core;
using StatusEffects;

namespace KillSkill.Skills.Implementations
{
    public class LifestealStatusEffect : TimedStatusEffect, IModifyIncomingDamage
    {
        private float healAmount;
        private ICharacter character;

        public LifestealStatusEffect(ICharacter character, float healAmount, float duration) : base(duration)
        {
            this.character = character;
            this.healAmount = healAmount;
        }

        public override StatusEffectDescription Description => new() {
            icon = SpriteDatabase.Get("status-lifesteal"),
            name = "Lifesteal",
            description = StandardDescription(healAmount)
        };

        public override void OnAdded(ICharacter target)
        {
            base.OnAdded(target);
            target.VisualEffects.Spawn("spore-pop", target.Position);
        }

        public static string StandardDescription(float healAmount) => $"Healing you for {healAmount} HP whenever you are dealing damage";

        public void ModifyDamage(ICharacter damager, ICharacter target, ref double damage)
        {
            int healMultiplier = 1;
            if(!damager.Resources.TryGet(out Bleed bleed))
                return;
                
            if(bleed.GetStack() <= 0) {
                damager.TryHeal(damager, healAmount);
                return;
            }
                
            bleed.RemoveStack(-1);
            damager.TryHeal(damager, healAmount * healMultiplier);
        }
    }
}