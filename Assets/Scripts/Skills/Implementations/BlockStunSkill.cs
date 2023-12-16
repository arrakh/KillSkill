using Actors;
using StatusEffects;

namespace Skills
{
    public class BlockStunSkill : Skill, IGlobalCooldownSkill
    {
        private float blockDuration;
        private float stunDuration;
        
        public override SkillDescription Description => new()
        {
            name = "Block Stun",
            description = $"Blocks for {blockDuration} seconds, then stuns for {stunDuration} when caster is hit",
            icon = SkillIconDatabase.Get("block-stun").icon
        };

        public BlockStunSkill(float cooldown, float blockDuration, float stunDuration) : base(cooldown)
        {
            this.blockDuration = blockDuration;
            this.stunDuration = stunDuration;
        }

        public override void Execute(Character caster, Character target)
        {
            caster.StatusEffects.Add(new BlockStunStatusEffect(blockDuration, stunDuration));
        }
    }
}