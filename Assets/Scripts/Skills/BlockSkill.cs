using Actors;
using StatusEffects;

namespace Skills
{
    public class BlockSkill : Skill
    {
        private float blockDuration;
        public override string DisplayName => "Block";

        public BlockSkill(float cooldown, float blockDuration) : base(cooldown)
        {
            this.blockDuration = blockDuration;
        }

        public override void Execute(Character caster, Character target)
        {
            caster.AddStatusEffect(new BlockStatusEffect(blockDuration));
        }
    }
}