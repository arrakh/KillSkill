using System.Collections.Generic;
using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.Database;
using Skills;

namespace KillSkill.Skills.Implementations
{
    public class VampiricSkill : Skill {
        private const float HEAL_AMOUNT = 16.75f;
        private const float DURATION = 3f;
        
        protected override float CooldownTime => 12f;

        public override SkillMetadata Metadata => new()
        {
            icon = SpriteDatabase.Get("status-lifesteal"),
            name = "Vampiric",
            description = $"Grants {DURATION} seconds of <u>Lifesteal</u>",
            extraDescription = $"- <u>Lifesteal</u>: {LifestealStatusEffect.StandardDescription(HEAL_AMOUNT)}"
        };

        public override CatalogEntry CatalogEntry => new()
        {
            order = 6, archetypeId = Archetypes.HEAVY_KNIGHT,
            resourceCosts = new Dictionary<string, double>()
            {
                {GameResources.COINS, 250},
                {GameResources.MEDALS, 2}
            }
        };

        public override void Execute(ICharacter caster, ICharacter target)
        {
            base.Execute(caster, target);
            target.StatusEffects.Add(new LifestealStatusEffect(caster, HEAL_AMOUNT, DURATION));
        }

    }
}