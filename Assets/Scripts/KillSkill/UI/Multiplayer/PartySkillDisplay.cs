using System;
using System.Collections.Generic;
using System.Linq;
using KillSkill.SessionData.Implementations;
using KillSkill.Skills;
using KillSkill.Utility;
using UnityEngine;

namespace KillSkill.UI.Multiplayer
{
    public class PartySkillDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject elementPrefab;
        [SerializeField] private RectTransform elementParent;

        private SkillsSessionData skillsSessionData;
        private List<SkillDisplay> spawnedElements = new();

        public void Display(SkillsSessionData skillsSession)
        {
            skillsSessionData = skillsSession;
            CleanObjects();
            
            var array = skillsSession.Loadout.ToArray();
            for (int i = 0; i < skillsSession.SlotCount; i++)
            {
                var type = array[i];

                Skill skill = null;
                if (type != null)
                {
                    var instance = Activator.CreateInstance(type);
                    if (instance != null) skill = instance as Skill;
                }
                
                var obj = Instantiate(elementPrefab, elementParent);
                var display = obj.GetComponent<SkillDisplay>();
                display.Display(skill, null); 
                
                if (skill.IsEmpty()) display.SetIsLocked(false);
                else display.SetIsLocked(!skillsSession.Owns(skill));
                
                spawnedElements.Add(display);
            }
        }

        private void CleanObjects()
        {
            foreach (var element in spawnedElements)
                Destroy(element.gameObject);
            
            spawnedElements.Clear();
        }
    }
}