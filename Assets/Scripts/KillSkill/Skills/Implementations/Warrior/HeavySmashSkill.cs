using System;
using System.Collections.Generic;
using DG.Tweening;
using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations;
using KillSkill.UI.Game;
using KillSkill.Utility;
using Skills;
using Range = KillSkill.Utility.Range;

namespace KillSkill.Skills.Implementations.Warrior
{
    public class HeavySmashSkill : Skill, IGlobalCooldownSkill, IAnySkillExecutedCallback, IHighlightSkill
    {
        private ICharacter casterChar;
        private ICharacter targetChar;

        private bool shouldCombo = false;
        
        [Configurable] private Range comboDamage = new (110f, 200f);
        [Configurable] private Range damage = new (30f, 45f);
        [Configurable] private float stanceDuration = 2.2f;
        [Configurable] private float stanceStaggerLimit = 20f;
        
        public event Action<bool> OnSetHighlight;

        protected override float CooldownTime => 5f;

        public override SkillMetadata Metadata => new()
        {
            icon = SpriteDatabase.Get("skill-smash"),
            name = "Heavy Smash",
            description = $"Enter </u>Stancing</u> for {stanceDuration} seconds, then deal {damage} damage. " +
                          $"\n\nWhen this ability is triggered after <u>Heavy Slam</u>, deal {comboDamage} damage",
            extraDescription = $"- <u>Stancing</u>: {StancingStatusEffect.StandardDescription(stanceStaggerLimit)}",
            isEmpty = false
        };

        public override CatalogEntry CatalogEntry => new()
        {
            order = 3, archetypeId = Archetypes.LEGACY_HEAVY_KNIGHT,
            resourceCosts = new Dictionary<string, double>()
            {
                {GameResources.COINS, 80}
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
            targetChar.TryDamage(casterChar, shouldCombo ? comboDamage.GetRandomRounded() : damage.GetRandomRounded());
        }

        public void OnAnyExecuted(ICharacter caster, ICharacter target, Skill skill)
        {
            if (skill is not IGlobalCooldownSkill) return;
            
            if (skill is HeavySmashSkill)
            {
                OnSetHighlight?.Invoke(false);
                return;
            }
            
            shouldCombo = skill is HeavySlamSkill;
            OnSetHighlight?.Invoke(shouldCombo);
        }
    }
}