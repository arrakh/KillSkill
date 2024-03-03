using Database;
using DG.Tweening;
using KillSkill.Characters;
using KillSkill.StatusEffects.Implementations.Core;
using StatusEffects;
using UnityEngine;

namespace KillSkill.StatusEffects.Implementations
{
    public class StunStatusEffect : TimerStatusEffect, IPreventCasting
    {
        public override void OnAdded(Character target)
        {
            var anim = target.Animator;
            var visual = anim.Visual;
            var sprite = anim.Sprite;

            sprite.color = new Color(1f, 0.75f, 0f, 1f);
            var color = sprite.DOColor(Color.white, RemainingDuration);
            var shake = visual.DOShakePosition(RemainingDuration, Vector3.right * 0.4f, 20);
            anim.AddTweens(shake, color);
            anim.PlayFlipBook("damaged");
        }

        public StunStatusEffect(float duration) : base(duration)
        {
        }

        public override StatusEffectDescription Description { get; } = new()
        {
            name = "Stunned",
            description = "Prevents casting any skill while active",
            icon = SpriteDatabase.Get("status-stunned")
        };
    }
}