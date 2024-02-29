using KillSkill.SessionData.Implementations;

namespace KillSkill.SessionData.Events
{
    public struct SkillsUpdatedEvent
    {
        public SkillsSessionData skillsSession;

        public SkillsUpdatedEvent(SkillsSessionData skillsSession)
        {
            this.skillsSession = skillsSession;
        }
    }
}