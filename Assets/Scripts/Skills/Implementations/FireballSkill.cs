using Actors;
using StatusEffects;

namespace Skills
{
    public class FireballSkill : Skill
    {
        private float damage;
        private float castingTime;
        private Character lastTarget;
        private Character lastCaster;
        private float burnDuration;
        private float burnDamage;

        public override string DisplayName => "Fireball";

        public FireballSkill(float damage, float castingTime, float burnDuration, float burnDamage, float cooldown) : base(cooldown)
        {
            this.damage = damage;
            this.castingTime = castingTime;
            this.burnDuration = burnDuration;
            this.burnDamage = burnDamage;
        }

        public override void Execute(Character caster, Character target)
        {
            lastCaster = caster;
            lastTarget = target;
            caster.AddStatusEffect(new CastingStatusEffect(castingTime, OnDoneCharging, null));
        }

        private void OnDoneCharging()
        {
            lastTarget.Damage(lastCaster, damage);
            lastTarget.AddStatusEffect(new BurningStatusEffect(burnDuration, burnDamage, lastCaster));
        }
    }
}