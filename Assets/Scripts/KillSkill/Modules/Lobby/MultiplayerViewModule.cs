using System.Threading.Tasks;
using Arr.EventsSystem;
using Arr.ViewModuleSystem;
using KillSkill.Modules.Network.Events;
using KillSkill.SessionData;
using KillSkill.SessionData.Events;
using KillSkill.SessionData.Implementations;
using KillSkill.UI.Multiplayer;
using KillSkill.UI.Navigation;
using KillSkill.UI.SkillsManager.Events;
using UnityEngine;

namespace KillSkill.Modules.Lobby
{
    public class MultiplayerViewModule : ViewModule<MultiplayerView>, INavigateSection,
        IEventListener<HostStartedEvent>,
        IEventListener<SessionUpdatedEvent<NetworkPartySessionData>>
    {
        private NetworkIdSessionData idSession;
        
        public void OnEvent(HostStartedEvent data)
        {
            view.SetLoading(false);
        }

        protected override async Task OnLoad()
        {
            await base.OnLoad();
            
            GlobalEvents.Fire(new AddNavigationEvent(this));

            idSession = Session.GetData<NetworkIdSessionData>();
            
            view.UpdateParty(Session.GetData<NetworkPartySessionData>(), idSession);
        }
        
        int INavigateSection.Order => 2;
        string INavigateSection.Name => "Multiplayer";
        void INavigateSection.OnNavigate(bool selected)
        {
            if (selected) view.Open();
            else view.Close();
        }

        public void OnEvent(SessionUpdatedEvent<NetworkPartySessionData> data)
        {
            view.UpdateParty(data.session, idSession);
        }
    }
}