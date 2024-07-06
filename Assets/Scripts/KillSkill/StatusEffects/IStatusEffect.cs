using KillSkill.Characters;
using StatusEffects;

namespace KillSkill.StatusEffects
{
    public interface IStatusEffect
    {
        public bool IsActive { get; }
        public StatusEffectDescription Description { get; }

        public void OnDuplicateAdded(ICharacter target, IStatusEffect duplicate);

        public void OnAdded(ICharacter target);
        public void OnUpdate(ICharacter target, float deltaTime);
        public void OnRemoved(ICharacter target);
    }
}