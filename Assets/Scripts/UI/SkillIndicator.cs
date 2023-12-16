using System;
using Actors;
using Skills;
using StatusEffects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SkillIndicator : MonoBehaviour
    {
        [SerializeField] private Character character;
        [SerializeField] private int skillIndex;
        [SerializeField] private GameObject cooldownGroup, lockedGroup, allGroup;
        [SerializeField] private Image fillImage, iconImage;
        [SerializeField] private TextMeshProUGUI cooldownText;
        [SerializeField] private Color skillCooldownColor, globalCooldownColor;

        private Skill skill;
        private bool usesGlobalCooldown;

        //SHOULD HAVE ON SKILL UPDATED EVENT, NOT ON UPDATE
        private void Update()
        {
            if (!character.TryGetSkill(skillIndex, out skill))
            {
               DisplayEmpty();
               return;
            }

            if (skill.Description.isEmpty)
            {
                DisplayEmpty();
                return;
            }
            
            if (!allGroup.activeInHierarchy) allGroup.SetActive(true);
            
            usesGlobalCooldown = skill is IGlobalCooldownSkill;

            bool preventedCasting = character.StatusEffects.Has<IPreventAbilityCasting>();
            lockedGroup.SetActive(preventedCasting);

            iconImage.sprite = skill.Description.icon;
            
            if (usesGlobalCooldown) HandleGlobalCooldownSkill();
            else HandleSkill();
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
            bool isOnCooldown = skill.Cooldown.IsActive || character.GlobalCooldown.IsActive;
            cooldownGroup.SetActive(isOnCooldown);
            if (!isOnCooldown) return;

            bool shouldDisplayGlobal = character.GlobalCooldown.RemainingTime > skill.Cooldown.RemainingTime;
            fillImage.fillAmount = shouldDisplayGlobal
                ? character.GlobalCooldown.NormalizedTime
                : skill.Cooldown.NormalizedTime;

            cooldownText.text = shouldDisplayGlobal
                ? character.GlobalCooldown.RemainingTime.ToString("F1")
                : skill.Cooldown.RemainingTime.ToString("F1");

            fillImage.color = shouldDisplayGlobal ? globalCooldownColor : skillCooldownColor;
        }

        public void DisplayEmpty()
        {
            allGroup.SetActive(false);
        }
    }
}