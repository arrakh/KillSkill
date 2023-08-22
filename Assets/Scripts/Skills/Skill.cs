using Actors;
using StatusEffects;
using UnityEngine;

namespace Skills
{
    public abstract class Skill
    {
        private readonly float cooldown;
        private float currentCooldownTime = 0f;

        public float NormalizedCooldown => Mathf.Clamp01(currentCooldownTime / cooldown);
        public float RemainingCooldownTime => Mathf.Clamp(currentCooldownTime, 0, cooldown);
        public bool IsOnCooldown => currentCooldownTime > 0f;

        public virtual string DisplayName => string.Empty;

        protected Skill(float cooldown)
        {
            this.cooldown = cooldown;
        }

        public void UpdateCooldown(float deltaTime)
        {
            currentCooldownTime -= deltaTime;
        }

        public bool CanExecute(Character caster) => !IsOnCooldown && !caster.HasAnyStatusEffect<IPreventAbilityCasting>();

        public void TriggerCooldown() => currentCooldownTime = cooldown;

        public virtual void Execute(Character caster, Character target) { }
    }
}
