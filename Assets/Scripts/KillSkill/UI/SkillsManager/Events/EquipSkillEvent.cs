using KillSkill.Skills;
using Skills;

namespace KillSkill.UI.SkillsManager.Events
{
    public struct EquipSkillEvent
    {
        public Skill skill;
        public int slotIndex;

        public EquipSkillEvent(Skill skill, int slotIndex)
        {
            this.skill = skill;
            this.slotIndex = slotIndex;
        }
    }
}