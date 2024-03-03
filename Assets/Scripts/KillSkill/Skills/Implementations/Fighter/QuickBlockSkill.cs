using System.Collections.Generic;
using Database;
using KillSkill.CharacterResources.Implementations;
using KillSkill.Characters;
using KillSkill.Constants;
using Skills;

namespace KillSkill.Skills.Implementations.Fighter
{
    public class QuickBlockSkill : Skill, IGlobalCooldownSkill
    {
        private const float SHIELD_AMOUNT = 30f;
        
        protected override float CooldownTime => 3f;

        public override SkillMetadata Metadata => new()
        {
            name = "Quick Block",
            description = $"Grants {SHIELD_AMOUNT} <u>Shield</u>",
            extraDescription = "- <u>Shield</u>:\nReduces damage when being attacked",
            icon = SpriteDatabase.Get("skill-quick-block")
        };

        public override CatalogEntry CatalogEntry => new()
        {
            archetypeId = Archetypes.FIGHTER,
            resourceCosts = new Dictionary<string, double>()
            {
                {GameResources.COINS, 100}
            },
            order = 1,
        };

        public override void Execute(Character caster, Character target)
        {
            var resources = caster.Resources;
                
            if (resources.TryGet<Shield>(out var existing)) existing.AddCharge(SHIELD_AMOUNT);
            else resources.Assign(new Shield(caster, SHIELD_AMOUNT));
        }
    }
}