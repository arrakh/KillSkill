using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations;
using KillSkill.Utility;
using Skills;

namespace KillSkill.Skills.Implementations.Assassin
{
    public class DodgeReflexSkill : Skill
    {
        protected override float CooldownTime => 18f;

        [Configurable] private float dodgeDuration = 12f;
        [Configurable] private float dodgeSuccessChance = 30f;
        [Configurable] private float dodgeMultiplier = 0.4f;

        public override SkillMetadata Metadata => new()
        {
            name = "Dodge Reflex",
            description = $"Grants <u>Dodging</u>: {DodgingStatusEffect.StandardDescription(dodgeSuccessChance, dodgeMultiplier)}",
            icon = SpriteDatabase.Get("skill-dodge-reflex")
        };
        
        public override CatalogEntry CatalogEntry => CatalogEntry.UnlockedFromStart(Archetypes.ASSASSIN, 1);

        public override void Execute(ICharacter caster, ICharacter target)
        {
            caster.StatusEffects.Add(new DodgingStatusEffect(dodgeDuration, dodgeSuccessChance, dodgeMultiplier));
        }
    }
}