using System;
using System.Linq;
using Arr.EventsSystem;
using KillSkill.Characters.Implementations;
using KillSkill.Modules.Battle.Events;
using KillSkill.SessionData.Implementations;
using KillSkill.SettingsData;
using KillSkill.Skills;
using KillSkill.Utility;
using UnityEngine;
using UnityEngine.Events;
using VisualEffects;

namespace KillSkill.Characters
{
    public class PlayerCharacter : Character
    {
        public UnityEvent<int> OnSkillIndexPressed;

        public void ServerInitialize(SkillsSessionData skillsSession, ICharacterFactory characterFactory)
        {
            var data = new CharacterData("mockup-player", 400, skillsSession.Loadout.ToArray());
            ServerInitialize(data, characterFactory);
        }

        protected override void OnClientInitialized()
        {
            if (IsOwner) GlobalEvents.Fire(new LocalPlayerInitializedEvent(this));
        }

        protected override void OnUpdate()
        {
            if (!IsOwner) return;
            
            var arr = Skills.GetAll();
            for (var i = 0; i < arr.Length; i++)
            {
                if (arr[i] == null) continue;
                var key = GameplaySettings.SkillBindings[i];
                if (key == KeyCode.None) continue;
                if (Input.GetKeyDown(key))
                {
                    OnSkillIndexPressed?.Invoke(i);
                    Skills.Execute(i, Target);
                }
            }
        }
    }
}