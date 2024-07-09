using System;
using Arr.EventsSystem;
using KillSkill.SessionData;
using KillSkill.SessionData.Events;
using KillSkill.SessionData.Implementations;
using KillSkill.UI.Arena;
using KillSkill.UI.Navigation;
using KillSkill.UI.SkillsManager;
using KillSkill.UI.SkillsManager.Events;
using SessionData.Implementations;
using UnityEngine;

namespace KillSkill.UI
{
    //todo: SHOULD BE SEPARATE MODULES
    public class MockupMenu : MonoBehaviour, 
        IEventListener<SessionUpdatedEvent<SkillsSessionData>>,
        IEventListener<EquipSkillEvent>,
        IEventListener<UnequipSkillEvent>,
        IEventListener<PurchaseSkillEvent>,
        IEventListener<DisplaySkillEvent>,
        IEventListener<PreviewBuySlotEvent>,
        IEventListener<BuySlotEvent>,
        IQueryProvider<DisplayedSkillQuery>
    {
        [SerializeField] private SkillsManagerView skillsManager;
        [SerializeField] private NavigationView navigationView;
        [SerializeField] private ArenaView arenaView;

        private ResourcesSessionData resourcesSession;
        private SkillsSessionData skillsSession;
        private BattleSessionData battleSession;
        private MilestonesSessionData milestonesSession;
        
        private void Start()
        {
            resourcesSession = Session.GetData<ResourcesSessionData>();
            skillsSession = Session.GetData<SkillsSessionData>();
            battleSession = Session.GetData<BattleSessionData>();
            milestonesSession = Session.GetData<MilestonesSessionData>();
            
            skillsManager.Display(resourcesSession, skillsSession);
            
            navigationView.AddSection(skillsManager);
            navigationView.AddSection(arenaView);
            navigationView.Select(skillsManager);
            arenaView.Display(battleSession, milestonesSession);
            GlobalEvents.Instance.RegisterMultiple(this);
        }

        public void OnEvent(SessionUpdatedEvent<SkillsSessionData> data)
        {
            skillsManager.Display(resourcesSession, skillsSession);
        }

        private void OnDestroy()
        {
            GlobalEvents.Instance.UnregisterMultiple(this);
        }

        public void OnEvent(EquipSkillEvent data)
        {
            if (skillsSession.IsEquipped(data.skill)) throw new Exception($"Trying to equip {data.skill.Metadata.name} but is already equipped");
            skillsSession.Equip(data.skill, data.slotIndex);
        }

        public void OnEvent(UnequipSkillEvent data)
        {
            if (!skillsSession.IsEquipped(data.skill)) throw new Exception($"Trying to unequip {data.skill.Metadata.name} but is not equipped");
            skillsSession.Unequip(data.skill);
        }

        public void OnEvent(PurchaseSkillEvent data)
        {
            var cost = data.skill.CatalogEntry.resourceCosts;
            
            if (!resourcesSession.CanAfford(cost))
            {
                skillsManager.AnimateCannotBuy();
                return;
            }

            resourcesSession.RemoveResources(cost);
            skillsSession.Add(data.skill.GetType());
        }

        public void OnEvent(DisplaySkillEvent data)
        {
            skillsManager.PreviewSkill(data.skill, skillsSession);
        }

        public DisplayedSkillQuery OnQuery()
        {
            var skill = skillsManager.GetCurrentlyDisplayed();
            if (skill != null) Debug.Log($"GOT: {skill.Metadata.name}");
            else Debug.Log("GOT NOTHING???");
            return new DisplayedSkillQuery()
            {
                success = skill != null,
                skill = skill
            };
        }

        public void OnEvent(PreviewBuySlotEvent data)
        {
            skillsManager.DisplayPurchaseSlot(skillsSession);
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
            skillsManager.DisplayPurchaseSlot(skillsSession);
        }
    }
}