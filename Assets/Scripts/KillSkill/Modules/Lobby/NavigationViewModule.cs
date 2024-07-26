using Arr.EventsSystem;
using Arr.ViewModuleSystem;
using KillSkill.UI.Navigation;
using KillSkill.UI.SkillsManager.Events;

namespace KillSkill.Modules.Lobby
{
    public class NavigationViewModule : ViewModule<NavigationView>,
        IEventListener<AddNavigationEvent>
    {
        public void OnEvent(AddNavigationEvent data)
        {
            view.AddSection(data.section);
            if (data.isDefaultSection) view.Select(data.section);
        }
    }
}