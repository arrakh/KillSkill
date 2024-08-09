using System.Threading.Tasks;
using Arr.EventsSystem;
using Arr.ViewModuleSystem;
using KillSkill.UI.Navigation;
using KillSkill.UI.SkillsManager.Events;

namespace KillSkill.Modules.Lobby
{
    public class NavigationViewModule : ViewModule<NavigationView>,
        IEventListener<AddNavigationEvent>
    {
        private INavigateSection defaultSection;
        
        public void OnEvent(AddNavigationEvent data)
        {
            view.AddSection(data.section);
            if (data.isDefaultSection) defaultSection = data.section;
        }

        protected override Task OnLoad()
        {
            view.Select(defaultSection);
            return base.OnLoad();
        }
    }
}