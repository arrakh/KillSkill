using DG.Tweening;
using KillSkill.CharacterResources.Implementations;
using KillSkill.Characters;
using KillSkill.StatusEffects.Implementations;
using Skills;
using StatusEffects;
using UnityEngine;

namespace KillSkill.Skills.Implementations.Enemy.Executioner
{
    public class SpinAttackSkill : Skill, IGlobalCooldownSkill
    {
        private const int SPIN_COUNT = 5;
        private const float DAMAGE = 3f;
        private const float INIT_PREP_TIME = 2f;
        private const float DURATION_PER_SPIN = 0.25f;
        protected override float CooldownTime => 6f;

        private Character casterChar;
        private Character targetChar;
        
        public override void Execute(Character caster, Character target)
        {
            casterChar = caster;
            targetChar = target;
            
            caster.Animator.PlayFlipBook("ChargeSpinAttack", 1f, null, false);
            caster.StatusEffects.Add(new CastingStatusEffect(INIT_PREP_TIME, OnDonePrep));
        }

        private void OnDonePrep()
        {
            Spin(0);
            casterChar.Animator.PlayFlipBook("SpinAttack");
            casterChar.StatusEffects.Add(new RepeatCastingStatusEffect(DURATION_PER_SPIN, SPIN_COUNT - 1, Spin));
        }

        private void Spin(int repeatIndex)
        {
            if (!targetChar.TryDamage(casterChar, DAMAGE)) return;

            if (targetChar.Resources.TryGet(out Bleed bleed))
            {
                bleed.AddStack(1);
                return;
            }
            
            targetChar.Resources.Assign(new Bleed(targetChar, 1));
        }
    }
}