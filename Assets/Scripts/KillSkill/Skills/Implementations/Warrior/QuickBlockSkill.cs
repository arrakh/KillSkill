using System.Collections.Generic;
using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations;
using KillSkill.Utility;
using Skills;

namespace KillSkill.Skills.Implementations.Warrior
{
    public class QuickBlockSkill : Skill, IGlobalCooldownSkill
    {
        protected override float CooldownTime => 3f;

        [Configurable] private float fortifiedDuration = 0.8f;
        [Configurable] private float fortifiedMultiplier = 0.2f;

        public override SkillMetadata Metadata => new()
        {
            name = "Quick Block",
            description = $"Grants <u>Fortify</u>: reduce incoming damage by {fortifiedMultiplier}x for {fortifiedDuration}s",
            icon = SpriteDatabase.Get("skill-quick-block")
        };

        public override CatalogEntry CatalogEntry => new()
        {
            archetypeId = Archetypes.WARRIOR,
            order = 4, resourceCosts = new Dictionary<string, double>()
            {
                {GameResources.COINS, 50}
            }
        };

        public override void Execute(ICharacter caster, ICharacter target)
        {
            caster.StatusEffects.Add(new FortifiedStatusEffect(fortifiedDuration, fortifiedMultiplier));
            target.VisualEffects.Spawn("quick-block", target.Position);
        }
    }
}