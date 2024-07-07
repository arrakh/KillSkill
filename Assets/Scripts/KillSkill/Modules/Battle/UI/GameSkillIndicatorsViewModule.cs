using System;
using Arr.EventsSystem;
using Arr.ViewModuleSystem;
using KillSkill.Characters;
using KillSkill.Modules.Battle.Events;

namespace KillSkill.UI.Battle.Modules
{
    public class GameSkillIndicatorsViewModule : ViewModule<GameSkillIndicatorsView>,
        IEventListener<BattleInitializedEvent>
    {
        public void OnEvent(BattleInitializedEvent data)
        {
            data.player.OnInitialize.Subscribe(OnInitialized);
        }

        private void OnInitialized(ICharacter character)
        {
            view.Display(character);
            if (character is not PlayerCharacter pc)
                throw new Exception("Initialized character is NOT player character?");
            pc.OnSkillIndexPressed.AddListener(view.AnimateSkill);
        }
    }
}