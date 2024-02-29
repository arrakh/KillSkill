using System;
using System.Linq;
using DefaultNamespace;
using KillSkill.SessionData;
using KillSkill.SessionData.Implementations;
using KillSkill.SettingsData;
using Skills;
using Unity.VisualScripting;
using UnityEngine;

namespace Actors
{
    public class Player : Character
    {
        [SerializeField] private Character monsterTarget;
         
        private void Start()
        {
            Initialize();

            var skillsSession = Session.GetData<SkillsSessionData>();

            skills = skillsSession.Loadout.ToArray();
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < skills.Length; i++)
            {
                if (skills[i] == null) continue;
                var key = GameplaySettings.SkillBindings[i];
                if (key == KeyCode.None) continue;
                if (Input.GetKeyDown(key)) ExecuteSkill(i, monsterTarget);
            }
        }
    }
}