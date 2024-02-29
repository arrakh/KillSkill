using System;
using System.Collections.Generic;
using Actors;
using Database;
using DG.Tweening;
using KillSkill.Constants;
using StatusEffects;
using UnityEngine;

namespace Skills
{
    public class AttackSkill : Skill, IGlobalCooldownSkill
    {
        protected override float CooldownTime => 2f;
        
        private const float DAMAGE = 20f;
        private const float CAST_TIME = 0.3f;
        private Character casterChar, targetChar;

        public override SkillMetadata Metadata => new()
        {
            name = "Attack",
            description = $"Basic attack dealing {DAMAGE} damage",
            icon = SpriteDatabase.Get("skill-attack")
        };
        
        public override CatalogEntry CatalogEntry => CatalogEntry.UnlockedFromStart(Archetypes.WARRIOR);

        public override void Execute(Character caster, Character target)
        {
            casterChar = caster;
            targetChar = target;
            caster.StatusEffects.Add(new CastingStatusEffect(CAST_TIME, OnDone, OnInterrupted));

            var cPos = casterChar.Animator.Visual.position;
            var tPos = targetChar.Animator.Visual.position;
            float distance = tPos.x - cPos.x;
            float realDistance = distance / 8f;
            Tween forward = casterChar.Animator.Visual.DOMoveX(cPos.x + realDistance, CAST_TIME).SetEase(Ease.OutQuart);
            
            casterChar.Animator.AddTweens(forward);
            casterChar.Animator.PlayFlipBook("attack");
        }

        private void OnInterrupted()
        {
            casterChar.Animator.BackToPosition();
        }

        private void OnDone()
        {
            targetChar.TryDamage(casterChar, DAMAGE);
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