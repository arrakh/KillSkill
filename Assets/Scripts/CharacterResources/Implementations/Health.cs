using System;
using Actors;
using StatusEffects;

namespace CharacterResources.Implementations
{
    public class Health : ICharacterResource
    {
        public event Action<double, double> OnHealthChanged; 

        private Character character;
        private double health, maxHealth;

        public double Max => maxHealth;

        public Health(Character character, double health, double maxHealth)
        {
            this.character = character;
            this.health = health;
            this.maxHealth = maxHealth;
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
            OnHealthChanged?.Invoke(oldHealth, health);
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

            var oldHealth = health;
            health += delta;
            Clamp();
            OnHealthChanged?.Invoke(oldHealth, health);
            return true;
        }

        private bool TryHarm(double delta, Character instigator)
        {
            foreach (var statusEffect in character.StatusEffects.GetAll())
                if(statusEffect is IModifyDamageStatusEffect modifier)
                    modifier.ModifyDamage(instigator, ref delta);
            
            var oldHealth = health;
            health -= delta;
            OnHealthChanged?.Invoke(oldHealth, health);
            if (health >= 0) return true;

            character.Kill();
            
            return true;
        }
    }
}