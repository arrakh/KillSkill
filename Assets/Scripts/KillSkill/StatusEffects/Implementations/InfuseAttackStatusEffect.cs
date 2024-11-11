using CharacterResources;
using KillSkill.StatusEffects.Implementations.Core;
using KillSkill.Utility;
using StatusEffects;

namespace KillSkill.StatusEffects.Implementations
{
    public class InfuseAttackStatusEffect<T> : TimedStatusEffect where T : ICharacterResource
    {
        [Configurable] private bool grantToTarget = true;
        [Configurable] private int amount = 1;
        
        public InfuseAttackStatusEffect(float duration) : base(duration)
        {
        }

        public override StatusEffectDescription Description { get; }
    }
}