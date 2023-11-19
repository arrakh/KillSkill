using Actors;
using StatusEffects;

namespace Skills
{
    public class LifestealSkill : Skill
    {
        private float healthAmount;
        private float statusDelay;
        private float statusDuration;

        public LifestealSkill(float cooldown, float healthAmount, float statusDelay, float statusDuration) : base(cooldown)
        {
            this.healthAmount = healthAmount;
            this.statusDelay = statusDelay;
            this.statusDuration = statusDuration;
        }

        public override string DisplayName => "Lifesteal";

        public override void Execute(Character caster, Character target)
        {
            target.StatusEffects.Add(new LifestealStatusEffect(caster, healthAmount, statusDelay, statusDuration));
        }
    }
}