using System.Collections.Generic;
using Database;
using DG.Tweening;
using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.StatusEffects.Implementations;
using Skills;

namespace KillSkill.Skills.Implementations.HeavyKnight
{
    public class SmashSkill : Skill, IGlobalCooldownSkill
    {
        private const float DAMAGE = 70f;
        private const float STANCE_TIME = 1.2f;
        private const float STANCE_THRESHOLD = 30f;

        private Character casterChar;
        private Character targetChar;

        protected override float CooldownTime => 4f;

        public override SkillMetadata Metadata => new()
        {
            icon = SpriteDatabase.Get("skill-smash"),
            name = "Smash",
            description = $"Enter </u>Stancing</u> for {STANCE_TIME} seconds, then deal {DAMAGE} damage",
            extraDescription = "<u>Stancing</u>:\n - ",
            isEmpty = false
        };

        public override CatalogEntry CatalogEntry => new()
        {
            order = 0, archetypeId = Archetypes.HEAVY_KNIGHT,
            resourceCosts = new Dictionary<string, double>()
            {
                {GameResources.COINS, 10}
            },
        };

        public override void Execute(Character caster, Character target)
        {
            casterChar = caster;
            targetChar = target;
            caster.AnimateMoveTowards(target, STANCE_TIME, Ease.OutQuart, -0.1f);
            caster.StatusEffects.Add(new StancingStatusEffect(OnStancingComplete, STANCE_THRESHOLD, STANCE_TIME));
        }

        private void OnStancingComplete()
        {
            casterChar.AnimateMoveTowards(targetChar, 0.15f, Ease.OutQuart, 0.3f, casterChar.Animator.BackToPosition);
            targetChar.TryDamage(casterChar, DAMAGE);
        }
    }
}