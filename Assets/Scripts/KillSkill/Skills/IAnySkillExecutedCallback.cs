using KillSkill.Characters;

namespace KillSkill.Skills
{
    public interface IAnySkillExecutedCallback
    {
        public void OnAnyExecuted(ICharacter caster, ICharacter target, Skill skill);
    }
}