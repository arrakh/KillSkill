using Actors;
using StatusEffects;
using UnityEngine;

namespace Skills
{
    public class ChargeAttackSkill : Skill, IGlobalCooldownSkill
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
            caster.StatusEffects.Add(new CastingStatusEffect(chargeDuration, OnDone, OnInterrupted));
        }

        private void OnInterrupted()
        {
            Debug.Log("INTERRUPTED!");
            casterChar.StatusEffects.Add(new StunStatusEffect(interruptedStunDuration));
        }

        private void OnDone()
        {
            targetChar.TryDamage(casterChar, damage);
        }
    }
}