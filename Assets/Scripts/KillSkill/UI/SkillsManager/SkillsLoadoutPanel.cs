using System;
using System.Collections.Generic;
using System.Linq;
using Arr.EventsSystem;
using KillSkill.SessionData.Implementations;
using KillSkill.SettingsData;
using KillSkill.Skills;
using KillSkill.UI.SkillsManager.Events;
using UnityEngine;

namespace KillSkill.UI.SkillsManager
{
    public class SkillsLoadoutPanel : MonoBehaviour
    {
        [SerializeField] private GameObject buySlot;
        [SerializeField] private GameObject elementPrefab;
        [SerializeField] private RectTransform elementParent;

        private SkillsSessionData skillsSessionData;
        private List<SkillsLoadoutSkillDisplay> spawnedElements = new();

        private void Update()
        {
            for (var i = 0; i < spawnedElements.Count; i++)
            {
                var key = GameplaySettings.SkillBindings[i];
                if (key == KeyCode.None) continue;
                if (Input.GetKeyDown(key)) TryEquipSelected(i);
            }
        }

        private void TryEquipSelected(int index)
        {
            Debug.Log($"WILL QUERY {index}");
            var query = GlobalEvents.Query<DisplayedSkillQuery>();
            if (!query.success) return;

            if (!skillsSessionData.Owns(query.skill)) return;
            
            skillsSessionData.Unequip(index);
            skillsSessionData.Equip(query.skill, index);
        }

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
                var display = obj.GetComponent<SkillsLoadoutSkillDisplay>();
                display.Display(skill, OnClicked);
                display.SetSlotIndex(i); 
                
                if (skill == null) display.SetIsLocked(false);
                else display.SetIsLocked(!skillsSession.Owns(skill));
                
                spawnedElements.Add(display);
            }
            
            buySlot.transform.SetAsLastSibling();
        }

        private void CleanObjects()
        {
            foreach (var element in spawnedElements)
                Destroy(element.gameObject);
            
            spawnedElements.Clear();
        }

        private void OnClicked(SkillDisplay display)
        {
            if (display.Skill == null) return;
            GlobalEvents.Fire(new DisplaySkillEvent(){skill = display.Skill});
        }
    }
}