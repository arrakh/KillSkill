using Arr.ViewModuleSystem;
using KillSkill.Battle;
using KillSkill.UI.Game.GameResult;

namespace KillSkill.UI.Game.Modules
{
    public class ResultViewModule : ViewModule<ResultView>
    {
        public void Display(BattleResultData result) => view.Display(result);
    }
}