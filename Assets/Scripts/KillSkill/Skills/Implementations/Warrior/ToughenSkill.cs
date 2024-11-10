using System.Collections.Generic;
using DG.Tweening;
using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations;
using KillSkill.Utility;
using Skills;

namespace KillSkill.Skills.Implementations.Warrior
{
    public class ToughenSkill : Skill
    {
        protected override float CooldownTime => 10f;

        [Configurable] private float fortifiedDuration = 5f;
        [Configurable] private float fortifiedMultiplier = 0.8f;

        public override SkillMetadata Metadata => new()
        {
            name = "Toughen",
            description = $"Grants <u>Fortify</u>: reduce incoming damage by {fortifiedMultiplier}x for {fortifiedDuration}s",
            icon = SpriteDatabase.Get("skill-toughen")
        };

        public override CatalogEntry CatalogEntry => new()
        {
            archetypeId = Archetypes.WARRIOR,
            order = 1, resourceCosts = new Dictionary<string, double>()
            {
                {GameResources.COINS, 20}
            }
        };

        public override void Execute(ICharacter caster, ICharacter target)
        {
            caster.StatusEffects.Add(new FortifiedStatusEffect(fortifiedDuration, fortifiedMultiplier));
        }
    }
}