using Actors;

namespace StatusEffects
{
    public class FrozenStatusEffect : StatusEffect
    {
        private float slowMultiplier;
        
        public FrozenStatusEffect(float slowMultiplier, float duration) : base(duration)
        {
            this.slowMultiplier = slowMultiplier;
        }

        public override void OnAdded(Character target)
        {
            target.SetCooldownSpeed(slowMultiplier);
        }

        public override void OnRemoved(Character target)
        {
            target.SetCooldownSpeed(1f);
        }
    }
}