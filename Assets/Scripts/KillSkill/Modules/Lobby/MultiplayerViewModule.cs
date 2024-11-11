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
            GlobalEvents.Fire(new AddNavigationEvent(this));

            idSession = Session.GetData<NetworkIdSessionData>();
            Debug.Log("MVM LOADED");

            view.UpdateParty(Session.GetData<NetworkPartySessionData>(), idSession);
            
            await base.OnLoad();
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
            //idSession = Session.GetData<NetworkIdSessionData>();
            view.UpdateParty(data.session, idSession);
        }
    }
}