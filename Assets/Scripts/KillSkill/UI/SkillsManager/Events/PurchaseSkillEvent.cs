using Skills;

namespace KillSkill.SessionData.Events
{
    public struct PurchaseSkillEvent
    {
        public Skill skill;

        public PurchaseSkillEvent(Skill skill)
        {
            this.skill = skill;
        }
    }
}