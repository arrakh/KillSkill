using Actors;
using UnityEngine;

namespace StatusEffects
{
    public class BurningStatusEffect : StatusEffect
    {
        private float lastDamageTime;
        private float dps;
        private Character damager;
        
        public BurningStatusEffect(float duration, float dps, Character damager) : base(duration)
        {
            this.dps = dps;
            this.damager = damager;
        }

        public override string DisplayName => "Burning";

        public override void OnUpdate(Character target)
        {
            var time = Time.time - lastDamageTime;
            if (time < 1f) return;

            lastDamageTime = Time.time;
            target.TryDamage(damager, dps);
        }
    }
}