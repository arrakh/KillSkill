using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations;
using KillSkill.Utility;
using Skills;

namespace KillSkill.Skills.Implementations.Assassin
{
    public class SharpEyesSkill : Skill
    {
        protected override float CooldownTime => 10f;

        [Configurable] private float critDuration = 6f;
        [Configurable] private float critSuccessChance = 20f;
        [Configurable] private float critMultiplier = 1.4f;

        public override SkillMetadata Metadata => new()
        {
            name = "Sharp Eyes",
            description = $"Grants <u>Critical Attack</u>: {CriticalAttackStatusEffect.StandardDescription(critSuccessChance, critMultiplier)}",
            icon = SpriteDatabase.Get("skill-sharp-eyes")
        };
        
        public override CatalogEntry CatalogEntry => CatalogEntry.UnlockedFromStart(Archetypes.ASSASSIN);

        public override void Execute(ICharacter caster, ICharacter target)
        {
            caster.StatusEffects.Add(new CriticalAttackStatusEffect(critDuration, critSuccessChance, critMultiplier));
        }
    }
}