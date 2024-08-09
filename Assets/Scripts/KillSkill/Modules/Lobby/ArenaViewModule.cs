using System.Threading.Tasks;
using Arr.EventsSystem;
using Arr.ViewModuleSystem;
using KillSkill.Modules.Network.Events;
using KillSkill.Network;
using KillSkill.Network.Messages;
using KillSkill.SessionData;
using KillSkill.SessionData.Implementations;
using KillSkill.UI.Arena;
using KillSkill.UI.Navigation;
using KillSkill.UI.SkillsManager.Events;
using UnityEngine;

namespace KillSkill.Modules.Lobby
{
    public class ArenaViewModule : ViewModule<ArenaView>, INavigateSection
    {
        private BattleSessionData battleSession;
        private MilestonesSessionData milestonesSession;

        protected override async Task OnLoad()
        {
            await base.OnLoad();
            
            battleSession = Session.GetData<BattleSessionData>();
            milestonesSession = Session.GetData<MilestonesSessionData>();
            
            view.Display(battleSession, milestonesSession);
            
            GlobalEvents.Fire(new AddNavigationEvent(this));
        }
        
        int INavigateSection.Order => 0;
        string INavigateSection.Name => "Arena";
        void INavigateSection.OnNavigate(bool selected)
        {
            if (selected) view.Open();
            else view.Close();
        }
    }
}