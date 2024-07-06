using System.Collections.Generic;
using KillSkill.CharacterResources.Implementations;
using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations;
using Skills;

namespace KillSkill.Skills.Implementations.Fighter
{
    public class SlashingShieldSkill : Skill
    {
        protected override float CooldownTime => 40f;

        private const float STATUS_DURATION = 7f;
        private const float SHIELD_CONVERT_PERCENT = 20f;

        public override CatalogEntry CatalogEntry => new()
        {
            archetypeId = Archetypes.FIGHTER,
            order = 3, resourceCosts = new Dictionary<string, double>()
            {
                {GameResources.COINS, 180},
                {GameResources.MEDALS, 1},
            }
        };

        public override SkillMetadata Metadata => new()
        {
            icon = SpriteDatabase.Get("skill-slashing-shield"),
            name = "Slashing Shield",
            description = "Grants <u>Slashing Shield</u>, increasing damage for every <u>Shield</u> the user has",
            extraDescription = "- <u>Slashing Shield</u> ??" +
                               $"\n- <u>Shield</u>:\n{Shield.StandardDescription()}"
        };

        public override void Execute(ICharacter caster, ICharacter target)
        {
            caster.StatusEffects.Add(new SlashingShieldStatusEffect(SHIELD_CONVERT_PERCENT, STATUS_DURATION));
        }
    }
}