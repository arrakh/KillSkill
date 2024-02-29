using System.Collections.Generic;
using Actors;
using Database;
using KillSkill.Constants;
using StatusEffects;

namespace Skills
{
    public class LifestealSkill : Skill
    {
        private const float HEAL_AMOUNT = 10;
        private const float STATUS_DELAY = 0.5f;
        private const float STATUS_DURATION = 3f;
        protected override float CooldownTime => 30f;
        public override CatalogEntry CatalogEntry => new(Archetypes.HEALER, new Dictionary<string, double>()
        {
            { GameResources.COINS, 100 }
        });

        public override SkillMetadata Metadata => new()
        {
            name = "Lifesteal",
            description = $"Steals {HEAL_AMOUNT} hp from the enemy",
            icon = SpriteDatabase.Get("skill-heal")
        };
        
        public override string DisplayName => "Lifesteal";

        public override void Execute(Character caster, Character target)
        {
            target.StatusEffects.Add(new LifestealStatusEffect(caster, HEAL_AMOUNT, STATUS_DELAY, STATUS_DURATION));
        }
    }
}