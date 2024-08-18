using System;
using System.Linq;
using Arr.EventsSystem;
using KillSkill.Characters.Implementations;
using KillSkill.Modules.Battle.Events;
using KillSkill.SessionData.Implementations;
using KillSkill.SettingsData;
using KillSkill.Skills;
using KillSkill.Utility;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using VisualEffects;

namespace KillSkill.Characters
{
    public class PlayerCharacter : Character
    {
        public UnityEvent<int> OnLocalSkillIndexPressed;

        public void ServerInitialize(uint characterId, SkillsSessionData skillsSession, ICharacterFactory factory)
        {
            var data = new CharacterData("mockup-player", 400, skillsSession.Loadout.ToArray());
            ServerInitialize(characterId, false, data, factory);
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
                    Debug.Log($"WILL EXECUTE IN SERVER SKILL INDEX {i}");
                    OnLocalSkillIndexPressed?.Invoke(i);
                    Skills.Execute(i, Target);
                }
            }
        }
    }
}