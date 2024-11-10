using DG.Tweening;
using KillSkill.Characters;
using KillSkill.StatusEffects;
using UnityEngine;

namespace StatusEffects
{
    /*public class BlockStunStatusEffect : IStatusEffect, IModifyDamageStatusEffect
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
            if (damager != null) damager.StatusEffects.Add(new StunStatusEffect(stunDuration));
        }
    }*/
}