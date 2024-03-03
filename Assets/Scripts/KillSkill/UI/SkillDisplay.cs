using System;
using KillSkill.Skills;
using Skills;
using UnityEngine;
using UnityEngine.UI;

namespace KillSkill.UI
{
    public class SkillDisplay : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private GameObject lockedGroup;
        [SerializeField] private Image highlightImage;
        [SerializeField] private Image flashImage;
        [SerializeField] private Button button;

        public Skill Skill => skill;
        
        protected Skill skill;
        private Action<SkillDisplay> onButtonClicked;

        public virtual void Display(Skill toDisplay, Action<SkillDisplay> onClicked)
        {
            skill = toDisplay;

            onButtonClicked = onClicked;
            
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnButtonClicked);
            
            if (skill == null)
            {
                DisplayEmpty();
                return;
            }

            var skillIcon = skill.Metadata.icon;
            if (skillIcon != null) icon.sprite = skillIcon;
            icon.enabled = skillIcon != null;
        }

        private void DisplayEmpty()
        {
            icon.enabled = false;
        }

        private void OnButtonClicked()
        {
            onButtonClicked?.Invoke(this);
        }

        public void SetIsLocked(bool locked) => lockedGroup.SetActive(locked);
        
        public void SetIsHighlighted(bool highlighted) => highlightImage.enabled = highlighted;
    }
}