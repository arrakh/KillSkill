using KillSkill.Characters;

namespace KillSkill.Skills
{
    public interface ISkillExecutedCallback<in T> where T : Skill
    {
        public void OnExecuted(Character caster, Character target, T skill);
    }
}