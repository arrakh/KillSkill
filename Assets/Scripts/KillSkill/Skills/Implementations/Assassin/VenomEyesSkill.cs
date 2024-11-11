using KillSkill.CharacterResources.Implementations;
using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations;
using KillSkill.Utility;
using Skills;

namespace KillSkill.Skills.Implementations.Assassin
{
    public class VenomEyesSkill : Skill
    {
        protected override float CooldownTime => 10f;
        
        [Configurable] private float infuseDuration = 4f;
        [Configurable] private int infuseAmount = 1;
        
        public override SkillMetadata Metadata => new()
        {
            name = "Venom Eyes",
            description = $"Grant <u>Infuse Attack: Poison</u>: Attacks has a chance to grant Poison to target",
            icon = SpriteDatabase.Get("skill-venom-eyes")
        };
        
        public override CatalogEntry CatalogEntry => CatalogEntry.UnlockedFromStart(Archetypes.ASSASSIN);

        public override void Execute(ICharacter caster, ICharacter target)
        {
            caster.StatusEffects.Add(new InfuseAttackStatusEffect<Poison>(infuseDuration, infuseAmount));
        }
    }
}