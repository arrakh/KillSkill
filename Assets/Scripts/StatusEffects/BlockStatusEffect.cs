using Actors;

namespace StatusEffects
{
    public class BlockStatusEffect : StatusEffect
    {
        public override string DisplayName => "Blocking";

        public BlockStatusEffect(float duration) : base(duration)
        {
        }

        public override void OnAdded(Character target)
        {
            
        }
    }
}