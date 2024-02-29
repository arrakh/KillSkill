using System.Collections.Generic;
using System.Linq;
using KillSkill.SessionData.Implementations;
using KillSkill.SettingsData;
using UnityEngine;

namespace KillSkill.UI.SkillsManager
{
    public class SkillsLoadoutPanel : MonoBehaviour
    {
        [SerializeField] private GameObject elementPrefab;
        [SerializeField] private RectTransform elementParent;

        private List<SkillsLoadoutSkillDisplay> spawnedElements = new();

        public void Display(SkillsSessionData skillsSession)
        {
            CleanObjects();
            
            var array = skillsSession.Loadout.ToArray();
            for (int i = 0; i < skillsSession.SlotCount; i++)
            {
                var skill = array[i];
                var obj = Instantiate(elementPrefab, elementParent);
                var display = obj.GetComponent<SkillsLoadoutSkillDisplay>();
                display.Display(skill, OnClicked);
                display.SetKeybinding(GameplaySettings.GetFormattedKeybinding(i)); 
                
                if (skill == null) display.SetIsLocked(false);
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

        private void OnClicked(SkillDisplay obj)
        {
            
        }
    }
}