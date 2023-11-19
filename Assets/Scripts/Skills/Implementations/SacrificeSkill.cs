using Actors;
using StatusEffects;

namespace Skills
{
    public class SacrificeSkill : Skill
    {
        private float healthSacrificeAmount;
        private float cooldownPercentReduction;
        private float statusEffectDuration;
        
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