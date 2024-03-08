using DG.Tweening;
using KillSkill.Characters;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations.Core;
using StatusEffects;
using UnityEngine;

namespace KillSkill.StatusEffects.Implementations
{
    public class StaggerStatusEffect : TimedStatusEffect, IPreventSkillExecution
    {
        public StaggerStatusEffect(float duration) : base(duration)
        {
        }

        public override void OnAdded(Character target)
        {
            base.OnAdded(target);
            var anim = target.Animator;
            var visual = anim.Visual;

            var shake = visual.DOShakePosition(0.6f, Vector3.right * 0.2f, 5);
            anim.AddTweens(shake);
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