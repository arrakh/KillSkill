using System.Collections.Generic;
using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations;
using Skills;

namespace KillSkill.Skills.Implementations.HeavyKnight
{
    public class PowerBurstSkill : Skill
    {
        private const float DAMAGE_MULT = 1.8f;
        private const float DURATION = 1.4f;
        
        protected override float CooldownTime => 15f;

        public override SkillMetadata Metadata => new()
        {
            icon = SpriteDatabase.Get("skill-power-burst"),
            name = "Power Burst",
            description = $"Grants {DURATION} seconds of <u>Empower</u>",
            extraDescription = $"- <u>Empower</u>: {EmpoweredStatusEffect.StandardDescription(DAMAGE_MULT)}"
        };

        public override CatalogEntry CatalogEntry => new()
        {
            order = 5, archetypeId = Archetypes.HEAVY_KNIGHT,
            resourceCosts = new Dictionary<string, double>()
            {
                {GameResources.COINS, 250},
                {GameResources.MEDALS, 2}
            }
        };

        public override void Execute(ICharacter caster, ICharacter target)
        {
            caster.StatusEffects.Add(new EmpoweredStatusEffect(DAMAGE_MULT, DURATION));
        }
    }
}