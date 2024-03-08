using KillSkill.Characters;
using KillSkill.SettingsData;
using KillSkill.Skills;
using Skills;
using StatusEffects;
using TMPro;
using UI.Tooltips;
using UnityEngine;
using UnityEngine.UI;

namespace KillSkill.UI.Game
{
    public class GameSkillIndicator : MonoBehaviour, ITooltipElement
    {
        [SerializeField] private GameObject cooldownGroup, lockedGroup, allGroup, highlightGroup, dimGroup;
        [SerializeField] private Image fillImage, iconImage;
        [SerializeField] private TextMeshProUGUI cooldownText, bindingText;
        [SerializeField] private Color skillCooldownColor, globalCooldownColor;
        [SerializeField] private SkillTapAnimation skillTapAnimation;

        private Character character;
        private Skill skill;
        private bool usesGlobalCooldown;
        private int skillIndex;

        //SHOULD HAVE ON SKILL UPDATED EVENT IF WE NEED TO CHANGE SKILL IN GAME
        
        private void Update()
        {
            if (skill == null) return;
            
            bool preventedCasting = character.StatusEffects.Has<IPreventSkillExecution>();
            lockedGroup.SetActive(preventedCasting);

            bool canExecute = skill.CanExecute(character);
            dimGroup.SetActive(!canExecute);

            if (usesGlobalCooldown) HandleGlobalCooldownSkill();
            else HandleSkill();
        }

        public void AnimateTap() => skillTapAnimation.Animate();

        public void Display(int slotIndex, Character toDisplay)
        {
            character = toDisplay;
            skillIndex = slotIndex;
            
            UpdateBinding();
            
            if (!character.Skills.TryGet(skillIndex, out var newSkill))
            {
                DisplayEmpty();
                return;
            }

            if (skill == null || newSkill != skill) OnNewSkill(newSkill);
            skill = newSkill;

            if (skill == null || skill.Metadata.isEmpty)
            {
                DisplayEmpty();
                return;
            }
            
            usesGlobalCooldown = skill is IGlobalCooldownSkill;
            
            var icon = skill.Metadata.icon;
            iconImage.sprite = icon;
        }

        private void OnNewSkill(Skill newSkill)
        {
            if (skill != null && skill is IHighlightSkill oldHighlight) oldHighlight.OnSetHighlight -= OnSetHighlight;
            if (newSkill is IHighlightSkill newHighlight) newHighlight.OnSetHighlight += OnSetHighlight;
        }

        void OnSetHighlight(bool highlight)
        {
            highlightGroup.SetActive(highlight);
        }

        public void UpdateBinding()
        {
            bindingText.text = GameplaySettings.GetFormattedKeybinding(skillIndex);
        }

        private void HandleSkill()
        {
            bool isOnCooldown = skill.Cooldown.IsActive;
            cooldownGroup.SetActive(isOnCooldown);
            if (!isOnCooldown) return;

            cooldownText.text = skill.Cooldown.RemainingTime.ToString("F1");
            fillImage.fillAmount = skill.Cooldown.NormalizedTime;
            fillImage.color = skillCooldownColor;
        }

        private void HandleGlobalCooldownSkill()
        {
            bool isOnCooldown = skill.Cooldown.IsActive || character.Skills.GlobalCooldown.IsActive;
            cooldownGroup.SetActive(isOnCooldown);
            if (!isOnCooldown) return;

            bool shouldDisplayGlobal = character.Skills.GlobalCooldown.RemainingTime > skill.Cooldown.RemainingTime;
            fillImage.fillAmount = shouldDisplayGlobal
                ? character.Skills.GlobalCooldown.NormalizedTime
                : skill.Cooldown.NormalizedTime;

            cooldownText.text = shouldDisplayGlobal
                ? character.Skills.GlobalCooldown.RemainingTime.ToString("F1")
                : skill.Cooldown.RemainingTime.ToString("F1");

            fillImage.color = shouldDisplayGlobal ? globalCooldownColor : skillCooldownColor;
        }

        public void DisplayEmpty()
        {
            allGroup.SetActive(false);
        }

        public bool HasData() => skill != null && !skill.Metadata.isEmpty;

        public TooltipData GetData()
        {
            var desc = skill.Metadata;
            return new TooltipData(desc.icon, desc.name, desc.description);
        }

        public int UniqueId => gameObject.GetInstanceID();
    }
}