using Skills;

namespace KillSkill.SessionData.Events
{
    public struct UnequipSkillEvent
    {
        public Skill skill;

        public UnequipSkillEvent(Skill skill)
        {
            this.skill = skill;
        }
    }
}