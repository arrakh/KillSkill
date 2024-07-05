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
            charPos.y += 1f;
            character.VisualEffects.Spawn("damage-hit", charPos);

            var damageText = Math.Round(damage).ToString("F1");
            var color = new Color(1f, 0.31f, 0.13f);

            ShowFlyingText(character, damageText, color, Vector3.up);
            
            return true;
        }

        public static void ShowFlyingText(this Character character, string text, Vector3 offsetPosition)
            => ShowFlyingText(character, text, Color.white, offsetPosition);
        
        public static void ShowFlyingText(this Character character, string text, Color color, Vector3 offsetPosition)
        {
            var charPos = character.transform.position;

            var flyingText = character.VisualEffects.Spawn("flying-text", charPos + offsetPosition)
                .GetEffectComponent<FlyingTextComponent>();
            
            flyingText.Display(text, 1f, color);
        }

        public static bool TryHeal(this Character character, Character healer, double heal)
        {
            //todo: Add effects like damage
            bool success = character.Resources.Get<Health>().TryHeal(ref heal, healer);
            return success;
        }
    }
}