using KillSkill.UI.Navigation;

namespace KillSkill.UI.SkillsManager.Events
{
    public struct AddNavigationEvent
    {
        public INavigateSection section;
        public bool isDefaultSection;

        public AddNavigationEvent(INavigateSection section, bool isDefaultSection = false)
        {
            this.section = section;
            this.isDefaultSection = isDefaultSection;
        }
    }
}