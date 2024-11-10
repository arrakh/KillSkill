using System.Collections.Generic;
using DG.Tweening;
using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations;
using KillSkill.Utility;
using Skills;

namespace KillSkill.Skills.Implementations.Warrior
{
    public class HeavySlamSkill : Skill, IGlobalCooldownSkill
    {
        private ICharacter casterChar;
        private ICharacter targetChar;
        
        [Configurable] private Range damage = new (20f, 40f);
        [Configurable] private float stanceDuration = 1.5f;
        [Configurable] private float stanceStaggerLimit = 20f;

        protected override float CooldownTime => 1.5f;

        public override SkillMetadata Metadata => new()
        {
            icon = SpriteDatabase.Get("skill-slam"),
            name = "Heavy Slam",
            description = $"Enter </u>Stancing</u> for {stanceDuration} seconds, then deal {damage} damage",
            extraDescription = $"- <u>Stancing</u>: {StancingStatusEffect.StandardDescription(stanceStaggerLimit)}",
            isEmpty = false
        };

        public override CatalogEntry CatalogEntry => new()
        {
            order = 2, archetypeId = Archetypes.WARRIOR,
            resourceCosts = new Dictionary<string, double>()
            {
                {GameResources.COINS, 30}
            },
        };

        public override void Execute(ICharacter caster, ICharacter target)
        {
            casterChar = caster;
            targetChar = target;
            caster.AnimateMoveTowards(target, stanceDuration, Ease.OutQuart, -0.1f);
            caster.StatusEffects.Add(new StancingStatusEffect(OnStancingComplete, stanceStaggerLimit, stanceDuration));
        }

        private void OnStancingComplete()
        {
            casterChar.AnimateMoveTowards(targetChar, 0.15f, Ease.OutQuart, 0.3f, casterChar.Animator.BackToPosition);
            targetChar.TryDamage(casterChar, damage.GetRandomRounded());
        }
    }
}