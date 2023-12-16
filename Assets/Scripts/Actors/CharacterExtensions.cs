using System;
using CharacterResources.Implementations;
using UnityEngine;
using VisualEffects.EffectComponents;

namespace Actors
{
    public static class CharacterExtensions
    {
        public static bool TryDamage(this Character character, Character damager, double damage)
        {
            var health = character.Resources.Get<Health>();
            bool success = health.TryAdd(-damage, damager);
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
            => character.Resources.Get<Health>().TryAdd(heal, healer);
    }
}