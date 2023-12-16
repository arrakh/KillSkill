using Actors;
using StatusEffects;
using UnityEngine;

namespace Skills
{
    public class SporePopSkill : Skill, IGlobalCooldownSkill
    {
        private float chargeDuration;
        private float damage;
        private float interruptedStunDuration;
        
        private Character casterChar;
        private Character targetChar;

        private bool charging = false;
        
        public override string DisplayName => "Charge Attack";

        public SporePopSkill(float cooldown, float chargeDuration, float damage, float interruptedStunDuration) : base(cooldown)
        {
            this.chargeDuration = chargeDuration;
            this.damage = damage;
            this.interruptedStunDuration = interruptedStunDuration;
        }

        public override void Execute(Character caster, Character target)
        {
            charging = true;
            casterChar = caster;
            targetChar = target;
            caster.StatusEffects.Add(new CastingStatusEffect(chargeDuration, OnDone, OnInterrupted));
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
            casterChar.StatusEffects.Add(new StunStatusEffect(interruptedStunDuration));
        }

        private void OnDone()
        {
            charging = false;
            if (!targetChar.TryDamage(casterChar, damage)) return;
            
            casterChar.Animator.PlayFlipBook("spore-pop");
            casterChar.VisualEffects.Spawn("spore-pop", casterChar.transform.position);
        }
    }
}