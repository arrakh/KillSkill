using Actors;
using StatusEffects;

namespace Skills
{
    public class BlockStunSkill : Skill, IGlobalCooldownSkill
    {
        private float blockDuration;
        private float stunDuration;
        public override string DisplayName => "Block";

        public BlockStunSkill(float cooldown, float blockDuration, float stunDuration) : base(cooldown)
        {
            this.blockDuration = blockDuration;
            this.stunDuration = stunDuration;
        }

        public override void Execute(Character caster, Character target)
        {
            caster.AddStatusEffect(new BlockStunStatusEffect(blockDuration, stunDuration));
        }
    }
}