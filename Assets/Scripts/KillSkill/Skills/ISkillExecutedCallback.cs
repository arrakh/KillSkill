using KillSkill.Characters;

namespace KillSkill.Skills
{
    public interface ISkillExecutedCallback<in T> where T : Skill
    {
        public void OnExecuted(ICharacter caster, ICharacter target, T skill);
    }
}