using Actors;
using StatusEffects;
using UnityEngine;

namespace Skills
{
    public class SporePopSkill : Skill, IGlobalCooldownSkill
    {
        private const float CHARGE_DURATION = 3f;
        private const float DAMAGE = 120f;
        private const float INTERRUPTED_STUN_DURATION = 5f;
        
        private Character casterChar;
        private Character targetChar;

        private bool charging = false;

        protected override float CooldownTime => 10f;

        public override void Execute(Character caster, Character target)
        {
            charging = true;
            casterChar = caster;
            targetChar = target;
            caster.StatusEffects.Add(new CastingStatusEffect(CHARGE_DURATION, OnDone, OnInterrupted));
            caster.Animator.PlayFlipBook("spore-charge-start", 1f, OnDoneChargeStart);
        }

        private void OnDoneChargeStart()
        {
            Debug.Log($"ON DONE CHARGE START! charging? {charging}");
            if (!charging) return;
            casterChar.Animator.PlayFlipBook("spore-charging");
        }

        private void OnInterrupted()
        {
            Debug.Log("INTERRUPTED!");
            charging = false;
            casterChar.StatusEffects.Add(new StunStatusEffect(INTERRUPTED_STUN_DURATION));
        }

        private void OnDone()
        {
            charging = false;
            if (!targetChar.TryDamage(casterChar, DAMAGE)) return;
            
            casterChar.Animator.PlayFlipBook("spore-pop");
            casterChar.VisualEffects.Spawn("spore-pop", casterChar.transform.position);
        }
    }
}