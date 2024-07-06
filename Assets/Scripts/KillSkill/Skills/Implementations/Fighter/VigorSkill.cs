using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.Database;
using Skills;

namespace KillSkill.Skills.Implementations.Fighter
{
    public class VigorSkill : Skill
    {
        private const float HEAL_AMOUNT = 50f;
        
        protected override float CooldownTime => 8f;
        
        public override SkillMetadata Metadata => new()
        {
            name = "Vigor",
            description = $"Quickly heals self for {HEAL_AMOUNT} HP",
            icon = SpriteDatabase.Get("skill-vigor")
        };
        
        public override CatalogEntry CatalogEntry => CatalogEntry.UnlockedFromStart(Archetypes.FIGHTER, int.MinValue + 1);

        public override void Execute(ICharacter caster, ICharacter target)
        {
            caster.TryHeal(caster, HEAL_AMOUNT);
        }
    }
}