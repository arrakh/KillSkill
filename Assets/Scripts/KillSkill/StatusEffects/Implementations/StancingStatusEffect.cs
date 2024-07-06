using System;
using KillSkill.CharacterResources.Implementations;
using KillSkill.Characters;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations.Core;
using StatusEffects;

namespace KillSkill.StatusEffects.Implementations
{
    public class StancingStatusEffect : TimedStatusEffect
    {
        public Action onComplete;
        public float staggerThreshold;

        public StancingStatusEffect(Action onComplete, float staggerThreshold, float duration) : base(duration)
        {
            this.onComplete = onComplete;
            this.staggerThreshold = staggerThreshold;
        }

        public override void OnAdded(ICharacter target)
        {
            target.Resources.Assign(new StanceStagger(target, staggerThreshold));
        }

        public override void OnRemoved(ICharacter target)
        {
            if (target.Resources.IsAssigned<StanceStagger>())
                target.Resources.Unassign<StanceStagger>();
            
            if (timer.IsActive) return;
            
            onComplete?.Invoke();
        }

        public override StatusEffectDescription Description => new()
        {
            icon = SpriteDatabase.Get("status-stancing"),
            name = "Stancing",
            description = StandardDescription(staggerThreshold)
        };

        public static string StandardDescription(float staggerThreshold) =>
            $"While active, getting damaged for {staggerThreshold} HP will put the user in <u>Stagger</u>";

        public static string StandardDescription() =>
            $"While active, getting damaged will put the user in <u>Stagger</u>";
    }
}