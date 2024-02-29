using System.Collections.Generic;
using Actors;
using Database;
using KillSkill.Constants;
using StatusEffects;

namespace Skills
{
    public class FireballSkill : Skill
    {
        private const float DAMAGE = 30f;
        private const float CASTING_TIME = 5f;
        private const float BURN_DURATION = 10f;
        private const float BURN_DAMAGE = 2f;
        protected override float CooldownTime => 10f;
        
        public override SkillMetadata Metadata => new()
        {
            name = "Fireball",
            description = $"Damages enemy for {DAMAGE}, then Burns enemy for {BURN_DAMAGE} hp every second for {BURN_DURATION} seconds",
            icon = SpriteDatabase.Get("skill-block-stun")
        };

        public override CatalogEntry CatalogEntry => new(Archetypes.MAGE, new Dictionary<string, double>()
        {
            {GameResources.COINS, 50}
        });

        private Character lastTarget;                      
        private Character lastCaster;

        public override void Execute(Character caster, Character target)
        {
            lastCaster = caster;
            lastTarget = target;
            caster.StatusEffects.Add(new CastingStatusEffect(CASTING_TIME, OnDoneCharging, null));
        }

        private void OnDoneCharging()
        {
            lastTarget.TryDamage(lastCaster, DAMAGE);
            lastTarget.StatusEffects.Add(new BurningStatusEffect(BURN_DURATION, BURN_DAMAGE, lastCaster));
        }
    }
}