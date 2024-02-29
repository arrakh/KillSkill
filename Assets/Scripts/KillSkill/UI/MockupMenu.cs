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
    public struct TestEvent{}
    
    public class MockupMenu : MonoBehaviour, 
        IEventListener<SkillsUpdatedEvent>,
        IEventListener<EquipSkillEvent>,
        IEventListener<UnequipSkillEvent>,
        IEventListener<PurchaseSkillEvent>,
        IEventListener<DisplaySkillEvent>
    {
        [SerializeField] private SkillsManagerView skillsManager;
        [SerializeField] private NavigationView navigationView;
        [SerializeField] private ArenaView arenaView;

        private ResourcesSessionData resourcesSession;
        private SkillsSessionData skillsSession;
        private void Start()
        {
            resourcesSession = Session.GetData<ResourcesSessionData>();
            skillsSession = Session.GetData<SkillsSessionData>();
            
            skillsManager.Display(resourcesSession, skillsSession);
            
            navigationView.AddSection(skillsManager);
            navigationView.AddSection(arenaView);
            navigationView.Select(skillsManager);
            GlobalEvents.Instance.RegisterMultiple(this);
        }

        public void OnEvent(SkillsUpdatedEvent data)
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
                Debug.LogError("CANNOT AFFORD");
                return;
            }

            resourcesSession.RemoveResources(cost);
            skillsSession.Add(data.skill.GetType());
        }

        public void OnEvent(DisplaySkillEvent data)
        {
            skillsManager.PreviewSkill(data.skill, skillsSession);
        }
    }
}