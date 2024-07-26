using System;
using System.Threading.Tasks;
using Arr.EventsSystem;
using Arr.ModulesSystem;
using KillSkill.SessionData;
using KillSkill.SessionData.Events;
using KillSkill.SessionData.Implementations;
using KillSkill.UI.SkillsManager.Events;

namespace KillSkill.Modules
{
    public class SkillSessionModule : BaseModule, 
        IEventListener<EquipSkillEvent>,
        IEventListener<UnequipSkillEvent>
    {
        private SkillsSessionData skillsSession;

        protected override async Task OnLoad()
        {
            skillsSession = Session.GetData<SkillsSessionData>();
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
    }
}