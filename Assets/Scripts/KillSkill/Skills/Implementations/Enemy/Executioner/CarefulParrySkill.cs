using KillSkill.Characters;
using KillSkill.StatusEffects.Implementations;
using StatusEffects;

namespace KillSkill.Skills.Implementations.Enemy.Executioner
{
    public class CarefulParrySkill : Skill
    {
        private const float CAST_DURATION = 0.5f;
        private const float PARRY_DURATION = 3f;
        private const float STAGGER_DURATION = 4f;
        private const float DAMAGE_REDUCTION = 0.2f;
        
        protected override float CooldownTime => 4f;
        private ICharacter casterChar;
        private ICharacter targetChar;

        public override void Execute(ICharacter caster, ICharacter target)
        {
            casterChar = caster;
            targetChar = target;
            caster.StatusEffects.Add(new CastingStatusEffect(CAST_DURATION, OnCast));
        }

        private void OnCast()
        {
            casterChar.StatusEffects.Add(new ParryingStatusEffect(casterChar, OnParryResult, DAMAGE_REDUCTION, PARRY_DURATION));
        }

        private void OnParryResult(bool result)
        {
            if (!result) return;
            
            targetChar.StatusEffects.Add(new StaggeredStatusEffect(STAGGER_DURATION));
        }
    }
}