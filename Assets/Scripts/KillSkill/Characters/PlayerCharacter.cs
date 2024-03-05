using System;
using System.Linq;
using KillSkill.Characters.Implementations.PlayerData;
using KillSkill.SessionData;
using KillSkill.SessionData.Implementations;
using KillSkill.SettingsData;
using KillSkill.Skills;
using UnityEngine;

namespace KillSkill.Characters
{
    public class PlayerCharacter : Character
    {
        private void Start()
        {
            var skillsSession = Session.GetData<SkillsSessionData>();

            var loadout = skillsSession.Loadout.ToArray();
            skills = new Skill[loadout.Length];
            
            for (var i = 0; i < loadout.Length; i++)
            {
                var skill = loadout[i];
                if (skill == null) skills[i] = null;
                else
                {
                    var instance = Activator.CreateInstance(skill) as Skill;
                    skills[i] = instance;
                }
            }

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