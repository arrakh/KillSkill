﻿using System;
using System.Linq;
using KillSkill.Characters.Implementations;
using KillSkill.SessionData.Implementations;
using KillSkill.SettingsData;
using KillSkill.Skills;
using UnityEngine;
using UnityEngine.Events;
using VisualEffects;

namespace KillSkill.Characters
{
    public class PlayerCharacter : Character
    {
        public UnityEvent<int> OnSkillIndexPressed;

        public void Initialize(SkillsSessionData skillsSession, ICharacterFactory characterFactory, 
            IVisualEffectsHandler visualEffectsHandler)
        {
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

            Initialize(new PlayerData(skills), characterFactory, visualEffectsHandler);
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
                    Skills.Execute(i, Target);
                }
            }
        }
    }
}