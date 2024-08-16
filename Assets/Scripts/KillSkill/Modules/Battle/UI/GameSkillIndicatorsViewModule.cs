using System;
using Arr.EventsSystem;
using Arr.ViewModuleSystem;
using KillSkill.Characters;
using KillSkill.Modules.Battle.Events;
using UnityEngine;

namespace KillSkill.UI.Battle.Modules
{
    public class GameSkillIndicatorsViewModule : ViewModule<GameSkillIndicatorsView>,
        IEventListener<LocalPlayerInitializedEvent>
    {
        public void OnEvent(LocalPlayerInitializedEvent data)
        {
            Debug.Log("TEST ON LOCAL INIT EVENT");
            data.localPlayer.OnInitialize.Subscribe(OnInitialized);
        }

        private void OnInitialized(ICharacter character)
        {
            Debug.Log("TEST ON LOCAL INITIALIZED, DISPLAYING CHARACTER");

            view.Display(character);
            if (character is not PlayerCharacter pc)
                throw new Exception("Initialized character is NOT player character?");
            pc.OnSkillIndexPressed.AddListener(view.AnimateSkill);
        }
    }
}