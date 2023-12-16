using Actors;
using StatusEffects;

namespace Skills
{
    public class SacrificeSkill : Skill
    {
        private float healthSacrificeAmount;
        private float cooldownPercentReduction;
        private float statusEffectDuration;
        
        public override SkillDescription Description => new()
        {
            name = "Sacrifice",
            description = $"Takes away {healthSacrificeAmount} hp from caster, then lowers cooldown by {cooldownPercentReduction}% for {statusEffectDuration} seconds",
            icon = SkillIconDatabase.Get("sacrifice").icon
        };
        
        public SacrificeSkill(float cooldown, float healthSacrificeAmount, float cooldownPercentReduction, float statusEffectDuration) : base(cooldown)
        {
            this.healthSacrificeAmount = healthSacrificeAmount;
            this.cooldownPercentReduction = cooldownPercentReduction;
            this.statusEffectDuration = statusEffectDuration;
        }

        public override string DisplayName => "Sacrifice";

        public override void Execute(Character caster, Character target)
        {
            caster.StatusEffects.Add(new CDSacrificeStatusEffect(statusEffectDuration, healthSacrificeAmount, cooldownPercentReduction));
        }
    }
}