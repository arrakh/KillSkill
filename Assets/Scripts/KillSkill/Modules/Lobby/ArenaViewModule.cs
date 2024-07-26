using System.Threading.Tasks;
using Arr.EventsSystem;
using Arr.ViewModuleSystem;
using KillSkill.SessionData;
using KillSkill.SessionData.Implementations;
using KillSkill.UI.Arena;
using KillSkill.UI.SkillsManager.Events;

namespace KillSkill.Modules.Lobby
{
    public class ArenaViewModule : ViewModule<ArenaView>
    {
        private BattleSessionData battleSession;
        private MilestonesSessionData milestonesSession;

        protected override async Task OnLoad()
        {
            await base.OnLoad();
            
            battleSession = Session.GetData<BattleSessionData>();
            milestonesSession = Session.GetData<MilestonesSessionData>();
            
            view.Display(battleSession, milestonesSession);
            
            GlobalEvents.Fire(new AddNavigationEvent(view));
        }
    }
}