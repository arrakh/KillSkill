using DG.Tweening;
using KillSkill.Characters;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations.Core;
using StatusEffects;
using UnityEngine;

namespace KillSkill.StatusEffects.Implementations
{
    public class StaggeredStatusEffect : TimedStatusEffect, IPreventSkillExecution
    {
        public StaggeredStatusEffect(float duration) : base(duration)
        {
        }

        public override void OnAdded(ICharacter target)
        {
            base.OnAdded(target);
            var anim = target.Animator;
            var visual = anim.Visual;

            var shake = visual.DOShakePosition(0.6f, Vector3.right * 0.2f, 5);
            anim.AddTweens(shake);
            
            target.StatusEffects.TryRemove<CastingStatusEffect>();
            target.StatusEffects.TryRemove<StancingStatusEffect>();
        }

        public override StatusEffectDescription Description => new()
        {
            name = "Staggered",
            description = StandardDescription(),
            icon = SpriteDatabase.Get("status-stagger")
        };

        public static string StandardDescription() => "Prevent triggering ability while active";
    }
}