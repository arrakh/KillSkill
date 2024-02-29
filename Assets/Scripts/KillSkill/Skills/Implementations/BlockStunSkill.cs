using Actors;
using Database;
using KillSkill.Constants;
using StatusEffects;

namespace Skills
{
    public class BlockStunSkill : Skill, IGlobalCooldownSkill
    {
        private const float BLOCK_DURATION = 2f;
        private const float STUN_DURATION = 4f;
        protected override float CooldownTime => 4f;
        public override CatalogEntry CatalogEntry => CatalogEntry.UnlockedFromStart(Archetypes.WARRIOR);

        public override SkillMetadata Metadata => new()
        {
            name = "Block Stun",
            description = $"Blocks for {BLOCK_DURATION} seconds, then stuns for {STUN_DURATION} when caster is hit",
            icon = SpriteDatabase.Get("skill-block-stun")
        };


        public override void Execute(Character caster, Character target)
        {
            caster.StatusEffects.Add(new BlockStunStatusEffect(BLOCK_DURATION, STUN_DURATION));
        }
    }
}