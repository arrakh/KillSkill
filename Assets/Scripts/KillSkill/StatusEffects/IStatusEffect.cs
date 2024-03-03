using KillSkill.Characters;
using StatusEffects;

namespace KillSkill.StatusEffects
{
    public interface IStatusEffect
    {
        public bool IsActive { get; }
        public StatusEffectDescription Description { get; }

        public void OnDuplicateAdded(Character target);

        public void OnAdded(Character target);
        public void OnUpdate(Character target, float deltaTime);
        public void OnRemoved(Character target);
    }
}