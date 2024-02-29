using System;
using System.Linq;
using Actors;
using StatusEffects;
using UI;
using UnityEngine;

namespace CharacterResources.Implementations
{
    public class Health : ICharacterResource, IResourceBarDisplay
    {
        public event Action<double, double> OnUpdated;

        private Character character;
        private double health, maxHealth;

        public double Max => maxHealth;

        public Health(Character character, double health, double maxHealth)
        {
            this.character = character;
            this.health = health;
            this.maxHealth = maxHealth;
        }
        
        public ResourceBarDisplaySettings GetDisplaySettings(Character character)
        {
            bool isPlayer = character is Player;
            var playerColor = new Color(165f / 255f, 1f, 97f / 255f);
            var enemyColor = new Color(1f, 25f / 255f, 25f / 255f);
            return new()
            {
                min = 0,
                max = Max,
                barColor = isPlayer ? playerColor : enemyColor
            };
        }

        public bool HasAny() => health > 0;

        public double Get() => health;

        private void Clamp()
        {
            if (health > maxHealth) health = maxHealth;
        }

        public bool TrySet(double value, Character instigator)
        {
            var oldHealth = health;
            health = value;
            Clamp();
            OnUpdated?.Invoke(oldHealth, health);
            return true;
        }

        public bool TryAdd(double delta, Character instigator)
        {
            if (instigator == null) return false;
            if (delta > 0f) return TryHeal(delta, instigator);
            if (delta < 0f) return TryHarm(-delta, instigator);
            return false;
        }
        
        private bool TryHeal(double delta, Character instigator)
        {
            foreach (var statusEffect in character.StatusEffects.GetAll())
                if(statusEffect is IModifyHealStatusEffect modifier)
                    modifier.ModifyHeal(instigator, ref delta);

            if (delta <= 0) return false;

            var oldHealth = health;
            health += delta;
            Clamp();
            OnUpdated?.Invoke(oldHealth, health);
            return true;
        }

        private bool TryHarm(double delta, Character instigator)
        {
            foreach (var statusEffect in character.StatusEffects.GetAll().ToList())
                if(statusEffect is IModifyDamageStatusEffect modifier)
                    modifier.ModifyDamage(instigator, ref delta);

            if (delta <= 0) return false;
            
            var oldHealth = health;
            health -= delta;
            OnUpdated?.Invoke(oldHealth, health);
            if (health > 0) return true;

            character.Kill();
            
            return true;
        }
    }
}