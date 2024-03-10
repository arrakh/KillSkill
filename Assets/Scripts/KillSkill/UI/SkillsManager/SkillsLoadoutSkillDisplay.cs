using System;
using KillSkill.SettingsData;
using KillSkill.Skills;
using Skills;
using TMPro;
using UI.Tooltips;
using UnityEngine;
using UnityEngine.UI;

namespace KillSkill.UI.SkillsManager
{
    public class SkillsLoadoutSkillDisplay : SkillDisplay, ITooltipElement
    {
        [SerializeField] private Button keybindingButton;
        [SerializeField] private TextMeshProUGUI keybindText;

        private int slotIndex;

        public int SlotIndex => slotIndex;
        
        int ITooltipElement.UniqueId => gameObject.GetInstanceID();
        
        public override void Display(Skill toDisplay, Action<SkillDisplay> onClicked)
        {
            base.Display(toDisplay, onClicked);
            
            keybindingButton.onClick.RemoveAllListeners();
            keybindingButton.onClick.AddListener(OnKeybinding);
        }

        private void OnKeybinding()
        {
            
        }

        public void SetSlotIndex(int index)
        {
            slotIndex = index;
            
            keybindText.text = GameplaySettings.GetFormattedKeybinding(index);
        }
        
        public bool HasData() => skill != null && !skill.Metadata.isEmpty;

        public TooltipData GetData()
        {
            var desc = skill.Metadata;
            return new TooltipData(desc.icon, desc.name, desc.description, desc.extraDescription);
        }
    }
}