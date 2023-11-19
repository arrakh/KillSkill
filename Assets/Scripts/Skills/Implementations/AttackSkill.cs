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

        public override string DisplayName => "Attack";

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
            float quartDist = distance / 4f;
            Tween forward = casterChar.Animator.Visual.DOMoveX(cPos.x + quartDist, castTime).SetEase(Ease.OutQuart);
            
            casterChar.Animator.AddTweens(forward);
        }

        private void OnInterrupted()
        {
            casterChar.Animator.BackToPosition();
        }

        private void OnDone()
        {
            targetChar.TryDamage(casterChar, damage);
            var tPos = targetChar.Animator.Visual.position.x;
            Tween move = casterChar.Animator.Visual.DOMoveX(tPos, 0.15f).OnComplete(casterChar.Animator.BackToPosition);
            casterChar.Animator.AddTweens(move);
        }
    }
}