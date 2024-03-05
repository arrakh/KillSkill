using Database;
using KillSkill.Characters;
using KillSkill.Constants;
using Skills;

namespace KillSkill.Skills.Implementations.Fighter
{
    public class VigorSkill : Skill, IGlobalCooldownSkill
    {
        private const float HEAL_AMOUNT = 60f;
        
        protected override float CooldownTime => 4f;
        
        public override SkillMetadata Metadata => new()
        {
            name = "Vigor",
            description = $"Quickly heals self for {HEAL_AMOUNT} HP",
            icon = SpriteDatabase.Get("skill-vigor")
        };
        
        public override CatalogEntry CatalogEntry => CatalogEntry.UnlockedFromStart(Archetypes.FIGHTER, int.MinValue + 1);

        public override void Execute(Character caster, Character target)
        {
            caster.TryHeal(caster, HEAL_AMOUNT);
        }
    }
}