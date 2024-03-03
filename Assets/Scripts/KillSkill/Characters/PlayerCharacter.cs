using System.Linq;
using KillSkill.Characters.Implementations.PlayerData;
using KillSkill.SessionData;
using KillSkill.SessionData.Implementations;
using KillSkill.SettingsData;
using UnityEngine;

namespace KillSkill.Characters
{
    public class PlayerCharacter : Character
    {
        private void Start()
        {
            var skillsSession = Session.GetData<SkillsSessionData>();

            skills = skillsSession.Loadout.ToArray();
            
            Initialize(new MockupPlayer());
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < skills.Length; i++)
            {
                if (skills[i] == null) continue;
                var key = GameplaySettings.SkillBindings[i];
                if (key == KeyCode.None) continue;
                if (Input.GetKeyDown(key)) ExecuteSkill(i, target);
            }
        }
    }
}