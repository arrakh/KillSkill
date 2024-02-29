using System.Collections.Generic;
using Actors;
using Database;
using KillSkill.Constants;
using StatusEffects;

namespace Skills
{
    public class SacrificeSkill : Skill
    {
        private const float HEALTH_SACRIFICE_AMOUNT = 50f;
        private const float COOLDOWN_PERCENT_REDUCTION = 0.5f;
        private const float STATUS_EFFECT_DURATION = 10f;
        
        protected override float CooldownTime => 20f;

        public override SkillMetadata Metadata => new()
        {
            name = "Sacrifice",
            description = $"Takes away {HEALTH_SACRIFICE_AMOUNT} hp from caster, then lowers cooldown by {COOLDOWN_PERCENT_REDUCTION}% for {STATUS_EFFECT_DURATION} seconds",
            icon = SpriteDatabase.Get("skill-sacrifice")
        };
        
        public override CatalogEntry CatalogEntry => new(Archetypes.WARRIOR, new Dictionary<string, double>()
        {
            { GameResources.COINS, 200 }
        });

        public override string DisplayName => "Sacrifice";

        public override void Execute(Character caster, Character target)
        {
            caster.StatusEffects.Add(new SacrificeStatusEffect(STATUS_EFFECT_DURATION, HEALTH_SACRIFICE_AMOUNT, COOLDOWN_PERCENT_REDUCTION));
        }
    }
}