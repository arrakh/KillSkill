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
    public class FinalStrikeSkill : Skill, IGlobalCooldownSkill, IAnySkillExecutedCallback, IHighlightSkill
    {
        private ICharacter casterChar;
        private ICharacter targetChar;
        private bool shouldCombo;
        
        protected override float CooldownTime => 6f;

        [Configurable] private Range comboDamage = new (300f, 500f);
        [Configurable] private Range damage = new (60f, 70f);
        [Configurable] private float damageFail = 10f;
        [Configurable] private float stanceDuration = 3f;
        [Configurable] private float stanceStaggerLimit = 10f;
        [Configurable] private float comboCastDuration = 0.6f;
        
        public event Action<bool> OnSetHighlight;
        
        public override SkillMetadata Metadata => new()
        {
            icon = SpriteDatabase.Get("skill-final-strike"),
            name = "Final Strike",
            description = $"Enter </u>Stancing</u> for {stanceDuration} seconds." +
                          $"\n\nWhen this ability is triggered after <u>Heavy Smash</u>, deal {damage} damage, " +
                          $"and after {comboCastDuration}, deal another {comboDamage} damage",
            extraDescription = $"- <u>Stancing</u>: {StancingStatusEffect.StandardDescription(stanceStaggerLimit)}",
            isEmpty = false
        };

        public override CatalogEntry CatalogEntry => new()
        {
            order = 8, archetypeId = Archetypes.LEGACY_HEAVY_KNIGHT,
            resourceCosts = new Dictionary<string, double>()
            {
                {GameResources.COINS, 200},
                {GameResources.MEDALS, 1},
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
            casterChar.AnimateMoveTowards(targetChar, 0.15f, Ease.OutQuart, 0.3f);

            if (!shouldCombo)
            {
                targetChar.TryDamage(casterChar, damageFail);
                return;
            }
            
            casterChar.StatusEffects.Add(new CastingStatusEffect(comboCastDuration, OnDoneComboCast));
            targetChar.TryDamage(casterChar, damage.GetRandomRounded());
        }

        private void OnDoneComboCast()
        {
            casterChar.AnimateMoveTowards(targetChar, 0.15f, Ease.OutQuart, 0.3f, casterChar.Animator.BackToPosition);
            targetChar.TryDamage(casterChar, comboDamage.GetRandomRounded());
        }

        public void OnAnyExecuted(ICharacter caster, ICharacter target, Skill skill)
        {
            if (skill is not IGlobalCooldownSkill) return;
            
            if (skill is FinalStrikeSkill)
            {
                OnSetHighlight?.Invoke(false);
                return;
            }
            
            shouldCombo = skill is HeavySmashSkill;
            OnSetHighlight?.Invoke(shouldCombo);
        }
    }
}