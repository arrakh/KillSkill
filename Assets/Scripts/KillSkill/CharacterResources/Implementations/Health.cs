using System;
using System.Linq;
using CharacterResources;
using KillSkill.Characters;
using StatusEffects;
using UI;
using UnityEngine;

namespace KillSkill.CharacterResources.Implementations
{
    public class Health : ICharacterResource, IResourceBarDisplay
    {
        public event Action<ResourceBarDisplay> OnUpdateDisplay;
        public ResourceBarDisplay DisplayData { get; private set; }

        private Character character;
        private double health, maxHealth;

        public double Max => maxHealth;

        public Health(Character character, double health, double maxHealth)
        {
            this.character = character;
            this.health = health;
            this.maxHealth = maxHealth;
            
            bool isPlayer = character is PlayerCharacter;
            var playerColor = new Color(165f / 255f, 1f, 97f / 255f);
            var enemyColor = new Color(1f, 25f / 255f, 25f / 255f);

            DisplayData = new()
            {
                value = health,
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
        
        private void UpdateDisplay()
        {
            DisplayData.value = health;
            OnUpdateDisplay?.Invoke(DisplayData);
        }
        
        public bool TrySet(double value, Character instigator)
        {
            health = value;
            Clamp();
            UpdateDisplay();
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
            foreach (var statusEffect in character.StatusEffects.GetAll().ToList())
                if(statusEffect is IModifyHeal modifier)
                    modifier.ModifyHeal(instigator, ref delta);
            
            foreach (var resource in character.Resources.GetAll().ToList())
                if(resource is IModifyHeal modifier)
                    modifier.ModifyHeal(instigator, ref delta);

            if (delta <= 0) return false;

            health += delta;
            Clamp();
            UpdateDisplay();

            return true;
        }

        private bool TryHarm(double delta, Character instigator)
        {
            foreach (var statusEffect in character.StatusEffects.GetAll().ToList())
                if(statusEffect is IModifyDamage modifier)
                    modifier.ModifyDamage(instigator, ref delta);
            
            foreach (var resource in character.Resources.GetAll().ToList())
                if(resource is IModifyDamage modifier)
                    modifier.ModifyDamage(instigator, ref delta);

            if (delta <= 0) return false;
            
            health -= delta;
            
            UpdateDisplay();

            if (health > 0) return true;

            character.Kill();
            
            return true;
        }
    }
}