using Actors;

namespace StatusEffects
{
    public class CDSacrificeStatusEffect : StatusEffect
    {
        private float healthSacrificeAmount;
        private float cooldownPercentReduction;

        private float lastCooldown;

        public CDSacrificeStatusEffect(float duration, float healthSacrificeAmount, float cooldownPercentReduction) : base(duration)
        {
            this.healthSacrificeAmount = healthSacrificeAmount;
            this.cooldownPercentReduction = cooldownPercentReduction;
        }

        public override string DisplayName => "Sacrificed Health";

        public override void OnAdded(Character target)
        {
            base.OnAdded(target);
            target.TryDamage(target, healthSacrificeAmount);
            lastCooldown = target.CooldownMultiplier;
            target.SetCooldownSpeed(cooldownPercentReduction);
        }

        public override void OnRemoved(Character target)
        {
            base.OnRemoved(target);
            target.SetCooldownSpeed(1f);
        }
    }
}