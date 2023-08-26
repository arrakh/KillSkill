using Actors;
using DefaultNamespace;
using StatusEffects;
using UnityEngine;

namespace Skills
{
    public abstract class Skill
    {
        private readonly Timer cd;
        
        public virtual string DisplayName => string.Empty;
        public Timer Cooldown => cd;

        protected Skill(float cooldown)
        {
            cd = new Timer(cooldown, false);
        }

        public void UpdateCooldown(float deltaTime)
        {
            cd.Update(deltaTime);
        }

        public bool CanExecute(Character caster) => !cd.IsActive && caster.CanCastAbility(this);

        public void TriggerCooldown() => cd.Reset();

        public virtual void Execute(Character caster, Character target) { }
    }
}
