using Actors;
using DG.Tweening;
using UnityEngine;

namespace StatusEffects
{
    public class BlockStunStatusEffect : StatusEffect, IModifyDamageStatusEffect
    {
        private float stunDuration;
        public override string DisplayName => "Blocking";

        public BlockStunStatusEffect(float duration, float stunDuration) : base(duration)
        {
            this.stunDuration = stunDuration;
        }   

        public override void OnAdded(Character target)
        {
            var anim = target.Animator;
            var visual = anim.Visual;

            var shake = visual.DOShakePosition(0.6f, Vector3.right * 0.2f, 5);
            anim.AddTweens(shake);
        }

        public void ModifyDamage(Character damager, ref double damage)
        {
            damage = 0;
            damager.StatusEffects.Add(new StunStatusEffect(stunDuration));
        }
    }
}