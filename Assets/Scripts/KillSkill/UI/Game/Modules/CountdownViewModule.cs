using Arr.ViewModuleSystem;

namespace KillSkill.UI.Game.Modules
{
    public class CountdownViewModule : ViewModule<CountdownView>
    {
        public void Count(int counter) => view.Count(counter);
    }
}