using System;
using DG.Tweening;
using KillSkill.CharacterResources.Implementations;
using UnityEngine;
using VisualEffects.EffectComponents;

namespace KillSkill.Characters
{
    public static class CharacterExtensions
    {
        public static void AnimateMoveTowards(this Character character, Character target, float time, Ease ease, float distanceModifier = 1f, Action onComplete = null)
        {
            var cPos = character.Animator.Visual.position;
            var tPos = target.Animator.Visual.position;
            float distance = tPos.x - cPos.x;
            float realDistance = distance * distanceModifier;
            
            Tween forward = character.Animator.Visual.DOMoveX(cPos.x + realDistance, time).SetEase(ease);
            if (onComplete != null) forward.OnComplete(() => { onComplete(); }); 
            character.Animator.AddMovementTweens(forward);
        }
        
        public static bool TryDamage(this Character character, Character damager, double damage)
        {
            var health = character.Resources.Get<Health>();
            bool success = health.TryDamage(ref damage, damager);
            if (!success) return false;

            var maxHealth = health.Max;
            
            var intensity = Mathf.Clamp01((float)(damage / (maxHealth / 4f)));
            character.Animator.Damage(intensity);

            var charPos = character.transform.position;
            character.VisualEffects.Spawn("damage-hit", charPos);

            charPos.y += 2f;
            var flyingText = character.VisualEffects.Spawn("flying-text", charPos)
                .GetEffectComponent<FlyingTextComponent>();

            var damageText = Math.Round(damage).ToString("F1");
            flyingText.Display(damageText, 1f, Color.red);

            return true;
        }

        public static bool TryHeal(this Character character, Character healer, double heal)
        {
            //todo: Add effects like damage
            bool success = character.Resources.Get<Health>().TryHeal(ref heal, healer);
            return success;
        }
    }
}