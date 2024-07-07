using Arr.ViewModuleSystem;
using KillSkill.Battle;
using KillSkill.UI.Battle.GameResult;

namespace KillSkill.UI.Battle.Modules
{
    public class ResultViewModule : ViewModule<ResultView>
    {
        public void Display(BattleResultData result) => view.Display(result);
    }
}