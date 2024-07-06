using System.Collections.Generic;
using KillSkill.CharacterResources.Implementations;
using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations;
using Skills;

namespace KillSkill.Skills.Implementations.Fighter
{
    public class VigorousShieldSkill : Skill
    {
        protected override float CooldownTime => 40f;

        private const float STATUS_DURATION = 7f;
        private const float SHIELD_CONVERT_PERCENT = 20f;

        public override CatalogEntry CatalogEntry => new()
        {
            archetypeId = Archetypes.FIGHTER,
            order = 4, resourceCosts = new Dictionary<string, double>()
            {
                {GameResources.COINS, 120},
                {GameResources.MEDALS, 1},
            }
        };

        public override SkillMetadata Metadata => new()
        {
            icon = SpriteDatabase.Get("skill-vigorous-shield"),
            name = "Vigorous Shield",
            description = "Grants <u>Vigorous Shield</u>, increasing heal for every <u>Shield</u> the user has",
            extraDescription = "- <u>Vigorous Shield</u> ??" +
                               $"\n- <u>Shield</u>:\n{Shield.StandardDescription()}"
        };

        public override void Execute(ICharacter caster, ICharacter target)
        {
            caster.StatusEffects.Add(new VigorousShieldStatusEffect(SHIELD_CONVERT_PERCENT, STATUS_DURATION));
        }
    }
}