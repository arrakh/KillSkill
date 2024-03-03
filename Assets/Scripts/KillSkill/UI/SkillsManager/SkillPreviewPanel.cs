using System;
using System.Collections.Generic;
using Arr.EventsSystem;
using KillSkill.SessionData.Events;
using KillSkill.SessionData.Implementations;
using KillSkill.Skills;
using KillSkill.UI.SkillsManager.Events;
using Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KillSkill.UI.SkillsManager
{
    public class SkillPreviewPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title, description, purchaseText;
        [SerializeField] private Image icon;
        [SerializeField] private ResourcesPanel resourcesPanel;
        [SerializeField] private RectTransform content;
        [SerializeField] private Button purchaseButton;
        [SerializeField] private GameObject emptyGroup, contentGroup;

        private bool skillOwned;
        private bool skillEquipped;
        private Skill skill;

        private void Awake()
        {
            purchaseButton.onClick.AddListener(OnPurchase);
        }

        public void UpdateDisplay(SkillsSessionData skillsSession)
        {
            if (skill == null) return;
            Display(skill, skillsSession);
        }

        public void Display(Skill toDisplay, SkillsSessionData skillSession)
        {
            SetEmpty(false);
            
            skill = toDisplay;
            var metadata = skill.Metadata;
            var entry = skill.CatalogEntry;

            title.text = metadata.name;
            description.text = metadata.description;
            icon.sprite = metadata.icon;

            skillOwned = skillSession.Owns(skill);
            skillEquipped = skillSession.IsEquipped(skill);


            var cost = skillOwned ? new Dictionary<string, double>() : entry.resourceCosts;
            resourcesPanel.Display(cost);
            

            purchaseText.text = skillEquipped ? "Unequip" : skillOwned ? "Equip" : "Buy";
            
            Invoke(nameof(DelayedRebuild), 0f);
        }

        private void OnPurchase()
        {
            if (skillEquipped) GlobalEvents.Fire(new UnequipSkillEvent(skill));
            else if (skillOwned) GlobalEvents.Fire(new EquipSkillEvent(skill, int.MaxValue));
            else GlobalEvents.Fire(new PurchaseSkillEvent(skill));
        }

        void DelayedRebuild()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);
        }

        public void SetEmpty(bool showEmpty)
        {
            emptyGroup.SetActive(showEmpty);
            contentGroup.SetActive(!showEmpty);
        }
    }
}