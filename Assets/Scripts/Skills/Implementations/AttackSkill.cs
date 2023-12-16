using System;
using Actors;
using DG.Tweening;
using StatusEffects;
using UnityEngine;

namespace Skills
{
    public class AttackSkill : Skill, IGlobalCooldownSkill
    {
        private float damage;
        private float castTime;
        private Character casterChar, targetChar;

        public override SkillDescription Description => new()
        {
            name = "Attack",
            description = $"Basic attack dealing {damage} damage",
            icon = SkillIconDatabase.Get("attack").icon
        };

        public AttackSkill(float castTime, float cooldown, float damage) : base(cooldown)
        {
            this.damage = damage;
            this.castTime = castTime;
        }

        public override void Execute(Character caster, Character target)
        {
            casterChar = caster;
            targetChar = target;
            caster.StatusEffects.Add(new CastingStatusEffect(castTime, OnDone, OnInterrupted));

            var cPos = casterChar.Animator.Visual.position;
            var tPos = targetChar.Animator.Visual.position;
            float distance = tPos.x - cPos.x;
            float realDistance = distance / 8f;
            Tween forward = casterChar.Animator.Visual.DOMoveX(cPos.x + realDistance, castTime).SetEase(Ease.OutQuart);
            
            casterChar.Animator.AddTweens(forward);
            casterChar.Animator.PlayFlipBook("attack");
        }

        private void OnInterrupted()
        {
            casterChar.Animator.BackToPosition();
        }

        private void OnDone()
        {
            targetChar.TryDamage(casterChar, damage);
            var tPos = targetChar.Animator.Visual.position.x;
            var cPos = casterChar.Animator.Visual.position.x;
            float distance = tPos - cPos;
            float realDistance = distance / 5f;
            Tween move = casterChar.Animator.Visual.DOMoveX(cPos + realDistance, 0.15f).OnComplete(casterChar.Animator.BackToPosition);
            casterChar.Animator.AddTweens(move);
            casterChar.Animator.PlayFlipBook("idle");
            
        }
    }
}