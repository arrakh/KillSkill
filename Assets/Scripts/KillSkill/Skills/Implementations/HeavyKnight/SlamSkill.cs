using System;
using System.Collections.Generic;
using DG.Tweening;
using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations;
using KillSkill.UI.Game;
using Skills;

namespace KillSkill.Skills.Implementations.HeavyKnight
{
    public class SlamSkill : Skill, IGlobalCooldownSkill
    {
        private const float DAMAGE = 30f;
        private const float STANCE_TIME = 1.5f;
        private const float STANCE_THRESHOLD = 30f;

        private ICharacter casterChar;
        private ICharacter targetChar;

        protected override float CooldownTime => 1.5f;

        public override SkillMetadata Metadata => new()
        {
            icon = SpriteDatabase.Get("skill-slam"),
            name = "Slam",
            description = $"Enter </u>Stancing</u> for {STANCE_TIME} seconds, then deal {DAMAGE} damage",
            extraDescription = $"- <u>Stancing</u>: {StancingStatusEffect.StandardDescription(STANCE_THRESHOLD)}",
            isEmpty = false
        };

        public override CatalogEntry CatalogEntry => new()
        {
            order = 0, archetypeId = Archetypes.HEAVY_KNIGHT,
            resourceCosts = new Dictionary<string, double>()
            {
                {GameResources.COINS, 30}
            },
        };

        public override void Execute(ICharacter caster, ICharacter target)
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