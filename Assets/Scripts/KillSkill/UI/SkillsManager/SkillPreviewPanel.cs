using System;
using System.Collections.Generic;
using Arr.EventsSystem;
using KillSkill.SessionData.Events;
using KillSkill.SessionData.Implementations;
using KillSkill.Skills;
using KillSkill.UI.SkillsManager.Events;
using KillSkill.Utility;
using Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KillSkill.UI.SkillsManager
{
    public class SkillPreviewPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title, description, extraDescription, purchaseText;
        [SerializeField] private Image icon;
        [SerializeField] private ResourcesPanel resourcesPanel;
        [SerializeField] private RectTransform content;
        [SerializeField] private Button purchaseButton;
        [SerializeField] private GameObject emptyGroup, contentGroup;
        [SerializeField] private GameObject extraDescSeparator;

        private bool skillOwned;
        private bool skillEquipped;
        private Skill skill;

        public Skill CurrentSkill => skill;

        public void UpdateDisplay(SkillsSessionData skillsSession)
        {
            if (skill.IsEmpty()) return;
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
            icon.enabled = true;

            skillOwned = skillSession.Owns(skill);
            skillEquipped = skillSession.IsEquipped(skill);


            var cost = skillOwned ? new Dictionary<string, double>() : entry.resourceCosts;
            resourcesPanel.Display(cost);

            purchaseText.text = skillEquipped ? "Unequip" : skillOwned ? "Equip" : "Buy";

            DisplayExtraDescription(toDisplay.Metadata.extraDescription);
            
            purchaseButton.onClick.RemoveAllListeners();
            purchaseButton.onClick.AddListener(OnPurchase);

            Invoke(nameof(DelayedRebuild), 0f);
        }

        private void DisplayExtraDescription(string extraDesc)
        {
            var hasExtraDesc = !String.IsNullOrEmpty(extraDesc);
            
            extraDescSeparator.SetActive(hasExtraDesc);
            extraDescription.gameObject.SetActive(hasExtraDesc);
            if (!hasExtraDesc) return;

            extraDescription.text = extraDesc;
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

        public void DisplayBuySlot(SkillsSessionData skillSession)
        {
            SetEmpty(false);

            title.text = "Buy Slot";
            description.text = "Buy an additional Skill slot";
            icon.enabled = false;
            purchaseText.text = "Buy";
            
            DisplayExtraDescription(string.Empty);

            Dictionary<string, double> cost = skillSession.GetSlotCost();
            resourcesPanel.Display(cost);
            
            purchaseButton.onClick.RemoveAllListeners();
            purchaseButton.onClick.AddListener(OnPurchaseSlot);
            
            Invoke(nameof(DelayedRebuild), 0f);
        }

        private void OnPurchaseSlot()
        {
            GlobalEvents.Fire(new BuySlotEvent());
        }
    }
}