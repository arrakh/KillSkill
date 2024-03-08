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
    public class SmashSkill : Skill, IGlobalCooldownSkill, IAnySkillExecutedCallback, IHighlightSkill
    {
        private const float COMBO_DAMAGE = 130f;
        private const float DAMAGE = 40f;
        private const float STANCE_TIME = 2.2f;
        private const float STANCE_THRESHOLD = 10f;

        private Character casterChar;
        private Character targetChar;

        private bool shouldCombo = false;
        
        public event Action<bool> OnSetHighlight;

        protected override float CooldownTime => 5f;

        public override SkillMetadata Metadata => new()
        {
            icon = SpriteDatabase.Get("skill-smash"),
            name = "Smash",
            description = $"Enter </u>Stancing</u> for {STANCE_TIME} seconds, then deal {DAMAGE} damage. " +
                          $"\n\nWhen this ability is triggered after <u>Slam</u>, deal {COMBO_DAMAGE} damage",
            extraDescription = $"- <u>Stancing</u>: {StancingStatusEffect.StandardDescription(STANCE_THRESHOLD)}",
            isEmpty = false
        };

        public override CatalogEntry CatalogEntry => new()
        {
            order = 1, archetypeId = Archetypes.HEAVY_KNIGHT,
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
            targetChar.TryDamage(casterChar, shouldCombo ? COMBO_DAMAGE : DAMAGE);
        }

        public void OnAnyExecuted(Character caster, Character target, Skill skill)
        {
            if (skill is not IGlobalCooldownSkill) return;
            
            if (skill is SmashSkill)
            {
                OnSetHighlight?.Invoke(false);
                return;
            }
            
            shouldCombo = skill is SlamSkill;
            OnSetHighlight?.Invoke(shouldCombo);
        }
    }
}