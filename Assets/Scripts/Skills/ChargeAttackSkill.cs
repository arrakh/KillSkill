using Actors;
using StatusEffects;
using UnityEngine;

namespace Skills
{
    public class ChargeAttackSkill : Skill
    {
        private float chargeDuration;
        private float damage;
        private float interruptedStunDuration;
        
        private Character casterChar;
        private Character targetChar;
        
        public override string DisplayName => "Charge Attack";

        public ChargeAttackSkill(float cooldown, float chargeDuration, float damage, float interruptedStunDuration) : base(cooldown)
        {
            this.chargeDuration = chargeDuration;
            this.damage = damage;
            this.interruptedStunDuration = interruptedStunDuration;
        }

        public override void Execute(Character caster, Character target)
        {
            casterChar = caster;
            targetChar = target;
            caster.AddStatusEffect(new ChargeStatusEffect(chargeDuration, OnDone, OnInterrupted));
        }

        private void OnInterrupted()
        {
            Debug.Log("INTERRUPTED!");
            casterChar.AddStatusEffect(new StunStatusEffect(interruptedStunDuration));
        }

        private void OnDone()
        {
            Debug.Log("DONEE!!");

            targetChar.Damage(damage);
            if (targetChar.HasStatusEffect<BlockStatusEffect>())
            {
                casterChar.AddStatusEffect(new StunStatusEffect(interruptedStunDuration));
                Debug.Log("BLOCKED!");
            }
        }
    }
}