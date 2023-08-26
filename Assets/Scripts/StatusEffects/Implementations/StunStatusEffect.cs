using Actors;
using DG.Tweening;
using UnityEngine;

namespace StatusEffects
{
    public class StunStatusEffect : StatusEffect, IPreventAbilityCasting
    {
        public override string DisplayName => "Stunned";

        public override void OnAdded(Character target)
        {
            var anim = target.Animator;
            var visual = anim.Visual;
            var sprite = anim.Sprite;

            sprite.color = new Color(1f, 0.75f, 0f, 1f);
            var color = sprite.DOColor(Color.white, RemainingDuration);
            var shake = visual.DOShakePosition(RemainingDuration, Vector3.right * 0.4f, 20);
            anim.AddTweens(shake, color);
        }

        public StunStatusEffect(float duration) : base(duration)
        {
        }
    }
}