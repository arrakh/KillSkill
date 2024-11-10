using KillSkill.Characters;
using KillSkill.StatusEffects.Implementations;
using Skills;
using StatusEffects;
using UnityEngine;

namespace KillSkill.Skills.Implementations.Enemy.Mushroom
{
    public class SporePopSkill : Skill, IGlobalCooldownSkill
    {
        private const float CHARGE_DURATION = 3f;
        private const float DAMAGE = 120f;
        
        private ICharacter casterChar;
        private ICharacter targetChar;

        private bool charging = false;

        protected override float CooldownTime => 14f;

        public override void Execute(ICharacter caster, ICharacter target)
        {
            charging = true;
            casterChar = caster;
            targetChar = target;
            caster.StatusEffects.Add(new CastingStatusEffect(CHARGE_DURATION, OnDone));
            caster.Animator.PlayFlipBook("spore-charge-start", 1f, OnDoneChargeStart);
        }

        private void OnDoneChargeStart()
        {
            Debug.Log($"ON DONE CHARGE START! charging? {charging}");
            if (!charging) return;
            casterChar.Animator.PlayFlipBook("spore-charging");
        }

        private void OnDone()
        {
            charging = false;
            if (!targetChar.TryDamage(casterChar, DAMAGE)) return;
            
            casterChar.Animator.PlayFlipBook("spore-pop");
            casterChar.VisualEffects.Spawn("spore-pop", casterChar.GameObject.transform.position);
        }
    }
}