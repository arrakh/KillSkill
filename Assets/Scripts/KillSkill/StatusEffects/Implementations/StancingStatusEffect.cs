using System;
using Database;
using KillSkill.CharacterResources.Implementations;
using KillSkill.Characters;
using KillSkill.StatusEffects.Implementations.Core;
using StatusEffects;

namespace KillSkill.StatusEffects.Implementations
{
    public class StancingStatusEffect : TimerStatusEffect
    {
        public Action onComplete;
        public float staggerThreshold;

        public StancingStatusEffect(Action onComplete, float staggerThreshold, float duration) : base(duration)
        {
            this.onComplete = onComplete;
            this.staggerThreshold = staggerThreshold;
        }

        public override void OnAdded(Character target)
        {
            target.Resources.Assign(new StanceStagger(target, staggerThreshold));
        }

        public override void OnRemoved(Character target)
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
            $"While active, getting damaged for {staggerThreshold} HP will put you in <u>Stagger</u>";
    }
}