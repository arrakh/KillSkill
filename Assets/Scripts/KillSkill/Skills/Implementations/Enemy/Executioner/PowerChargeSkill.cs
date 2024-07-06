using System.Collections.Generic;
using KillSkill.Characters;
using KillSkill.Constants;
using KillSkill.Database;
using KillSkill.StatusEffects.Implementations;
using Skills;

namespace KillSkill.Skills.Implementations.Enemy.Executioner
{
    public class PowerChargeSkill : Skill
    {
        private const float DAMAGE_MULT = 1.2f;
        private const float DURATION = 2f;
        
        protected override float CooldownTime => 10f;

        public override void Execute(ICharacter caster, ICharacter target)
        {
            caster.StatusEffects.Add(new EmpoweredStatusEffect(DAMAGE_MULT, DURATION));
        }
    }
}