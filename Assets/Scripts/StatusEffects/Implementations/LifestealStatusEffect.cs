using Actors;
using UnityEngine;

namespace StatusEffects
{
    public class LifestealStatusEffect : StatusEffect
    {
        private float healthAmount, delay;
        private float lastDamageTime;
        private Character damager;
        
        public LifestealStatusEffect(Character damager, float healthAmount, float delay, float duration) : base(duration)
        {
            this.healthAmount = healthAmount;
            this.delay = delay;
            this.damager = damager;
        }

        public override string DisplayName => "Life Stolen!";

        public override void OnUpdate(Character target)
        {
            var time = Time.time - lastDamageTime;
            if (time < delay) return;

            lastDamageTime = Time.time;
            target.TryDamage(damager, healthAmount);
            damager.TryHeal(damager, healthAmount);
        }
    }
}