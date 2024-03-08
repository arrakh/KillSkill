using System;
using System.Linq;
using KillSkill.Characters.Implementations.PlayerData;
using KillSkill.SessionData;
using KillSkill.SessionData.Implementations;
using KillSkill.SettingsData;
using KillSkill.Skills;
using UnityEngine;
using UnityEngine.Events;

namespace KillSkill.Characters
{
    public class PlayerCharacter : Character
    {
        public UnityEvent<int> OnSkillIndexPressed;

        private void Start()
        {
            var skillsSession = Session.GetData<SkillsSessionData>();

            var loadout = skillsSession.Loadout.ToArray();
            var skills = new Skill[loadout.Length];
            
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

            Initialize(new MockupPlayer(), skills);
        }

        protected override void OnUpdate()
        {
            var arr = Skills.GetAll();
            for (var i = 0; i < arr.Length; i++)
            {
                if (arr[i] == null) continue;
                var key = GameplaySettings.SkillBindings[i];
                if (key == KeyCode.None) continue;
                if (Input.GetKeyDown(key))
                {
                    OnSkillIndexPressed?.Invoke(i);
                    Skills.Execute(i, target);
                }
            }
        }
    }
}