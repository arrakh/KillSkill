using System;
using System.Collections.Generic;
using Arr.EventsSystem;
using KillSkill.Database;
using KillSkill.SessionData.Events;
using KillSkill.SessionData.Implementations;
using KillSkill.Skills;
using KillSkill.UI.SkillsManager.Events;
using Skills;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace KillSkill.UI.SkillsManager
{
    public class SkillCatalogArchetypeElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText, descriptionText;
        [SerializeField] private Image bannerBg, contentBg;

        [FormerlySerializedAs("element")] [SerializeField] private GameObject elementPrefab;
        [SerializeField] private Transform elementParent;

        private Dictionary<Type, SkillCatalogSkillDisplay> spawnedElements = new();

        private SkillsSessionData skills;

        public void Display(SkillsSessionData skillsSession, ArchetypeData data)
        {
            skills = skillsSession;
            nameText.text = data.name;
            descriptionText.text = data.description;
            bannerBg.color = data.bannerColor;
            contentBg.color = data.contentColor;
        }

        public void AddSkill(Skill skill)
        {
            var obj = Instantiate(elementPrefab, elementParent);
            var display = obj.GetComponent<SkillCatalogSkillDisplay>();
            
            display.Display(skill, OnClicked);
            display.SetIsLocked(!skills.Owns(skill));

            spawnedElements[skill.GetType()] = display;
        }

        public bool TryGetSkillDisplay(Type skillType, out SkillCatalogSkillDisplay display)
            => spawnedElements.TryGetValue(skillType, out display);

        public void SetHighlights(Skill skill)
        {
            var skillType = skill.GetType();
            foreach (var (type, element) in spawnedElements)
                element.SetIsHighlighted(type == skillType);
        }

        private void OnClicked(SkillDisplay display)
        {
            Debug.Log("BUTTON CLICKEDD");
            GlobalEvents.Fire(new DisplaySkillEvent(){ skill = display.Skill });
        }
    }
}