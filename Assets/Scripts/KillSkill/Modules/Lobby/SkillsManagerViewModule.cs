using System.Threading.Tasks;
using Arr.EventsSystem;
using Arr.ViewModuleSystem;
using KillSkill.SessionData;
using KillSkill.SessionData.Events;
using KillSkill.SessionData.Implementations;
using KillSkill.UI.Navigation;
using KillSkill.UI.SkillsManager;
using KillSkill.UI.SkillsManager.Events;
using SessionData.Implementations;
using UnityEngine;

namespace KillSkill.Modules.Lobby
{
    public class SkillsManagerViewModule : ViewModule<SkillsManagerView>, INavigateSection,
        
        IEventListener<SessionUpdatedEvent<SkillsSessionData>>,
        IEventListener<DisplaySkillEvent>,
        IEventListener<PurchaseSkillEvent>,
        IEventListener<BuySlotEvent>,
        IEventListener<PreviewBuySlotEvent>,
        
        IQueryProvider<DisplayedSkillQuery>
    {
        private ResourcesSessionData resourcesSession;
        private SkillsSessionData skillsSession;
        
        protected override async Task OnLoad()
        {
            await base.OnLoad();

            resourcesSession = Session.GetData<ResourcesSessionData>();
            skillsSession = Session.GetData<SkillsSessionData>();
            
            view.Display(resourcesSession, skillsSession);
            
            GlobalEvents.Fire(new AddNavigationEvent(this, true));
        }
        
        public void OnEvent(SessionUpdatedEvent<SkillsSessionData> data)
            => view.Display(resourcesSession, skillsSession);
        
        public void OnEvent(PurchaseSkillEvent data)
        {
            var cost = data.skill.CatalogEntry.resourceCosts;
            
            if (!resourcesSession.CanAfford(cost))
            {
                view.AnimateCannotBuy();
                return;
            }

            resourcesSession.RemoveResources(cost);
            skillsSession.Add(data.skill.GetType());
        }
        
        public void OnEvent(DisplaySkillEvent data)
        {
            view.PreviewSkill(data.skill, skillsSession);
        }
        
        public void OnEvent(PreviewBuySlotEvent data)
        {
            view.DisplayPurchaseSlot(skillsSession);
        }

        public DisplayedSkillQuery OnQuery()
        {
            var skill = view.GetCurrentlyDisplayed();
            if (skill != null) Debug.Log($"GOT: {skill.Metadata.name}");
            else Debug.Log("GOT NOTHING???");
            return new DisplayedSkillQuery()
            {
                success = skill != null,
                skill = skill
            };
        }
        
        public void OnEvent(BuySlotEvent data)
        {
            var cost = skillsSession.GetSlotCost();
            
            if (!resourcesSession.CanAfford(cost))
            {
                Debug.LogError("CANNOT AFFORD");
                return;
            }

            resourcesSession.RemoveResources(cost);
            skillsSession.AddSlot(1);
            view.DisplayPurchaseSlot(skillsSession);
        }
        
        int INavigateSection.Order => 1;
        string INavigateSection.Name => "Skills";
        void INavigateSection.OnNavigate(bool selected)
        {
            if (selected) view.Open();
            else view.Close();
        }
    }
}