using KillSkill.Characters;

namespace KillSkill.Skills
{
    public interface IAnySkillExecutedCallback
    {
        public void OnAnyExecuted(Character caster, Character target, Skill skill);
    }
}