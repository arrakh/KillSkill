using Arr.ViewModuleSystem;
using KillSkill.UI.Navigation;

namespace KillSkill.UI.Arena
{
    public class ArenaView : View, INavigateSection
    {
        //todo: Should sit in the view module
        int INavigateSection.Order => 0;
        string INavigateSection.Name => "Arena";
        void INavigateSection.OnNavigate(bool selected)
        {
            gameObject.SetActive(selected);
        }
    }
}