using System.Collections.Generic;
using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations;
using KillSkill.Utility;
using Skills;

namespace KillSkill.Skills.Implementations.Warrior
{
    public class PowerBurstSkill : Skill
    {
        [Configurable] private float empowerDuration = 1.4f;
        [Configurable] private float empowerMultiplier = 1.8f;
        
        protected override float CooldownTime => 15f;

        public override SkillMetadata Metadata => new()
        {
            icon = SpriteDatabase.Get("skill-power-burst"),
            name = "Power Burst",
            description = $"Grants {empowerDuration} seconds of <u>Empower</u>",
            extraDescription = $"- <u>Empower</u>: {EmpoweredStatusEffect.StandardDescription(empowerMultiplier)}"
        };

        public override CatalogEntry CatalogEntry => new()
        {
            order = 5, archetypeId = Archetypes.LEGACY_HEAVY_KNIGHT,
            resourceCosts = new Dictionary<string, double>()
            {
                {GameResources.COINS, 250},
                {GameResources.MEDALS, 2}
            }
        };

        public override void Execute(ICharacter caster, ICharacter target)
        {
            caster.StatusEffects.Add(new EmpoweredStatusEffect(empowerMultiplier, empowerDuration));
        }
    }
}