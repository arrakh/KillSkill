using Arr.ViewModuleSystem;
using KillSkill.UI.Game;

namespace KillSkill.UI.Battle.Modules
{
    public class CountdownViewModule : ViewModule<CountdownView>
    {
        public void Count(int counter) => view.Count(counter);
    }
}