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
        [SerializeField] private GameObject cooldownGroup, lockedGroup;
        [SerializeField] private Image fillImage;
        [SerializeField] private Slider cooldownSlider;
        [SerializeField] private TextMeshProUGUI cooldownText;
        [SerializeField] private TextMeshProUGUI skillNameText;
        [SerializeField] private Color skillCooldownColor, globalCooldownColor;

        private Skill skill;
        private bool usesGlobalCooldown;

        private void Start()
        {
            
        }

        private void Update()
        {
            skill = character.GetSkill(skillIndex);
            usesGlobalCooldown = skill is IGlobalCooldownSkill;
            skillNameText.text = skill.DisplayName;

            bool preventedCasting = character.HasStatusEffect<IPreventAbilityCasting>();
            lockedGroup.SetActive(preventedCasting);
            
            if (usesGlobalCooldown) HandleGlobalCooldownSkill();
            else HandleSkill();
        }

        private void HandleSkill()
        {
            bool isOnCooldown = skill.Cooldown.IsActive;
            cooldownGroup.SetActive(isOnCooldown);
            if (!isOnCooldown) return;

            cooldownText.text = skill.Cooldown.RemainingTime.ToString("F1");
            cooldownSlider.value = skill.Cooldown.NormalizedTime;
            fillImage.color = cooldownText.color = skillCooldownColor;
        }

        private void HandleGlobalCooldownSkill()
        {
            bool isOnCooldown = skill.Cooldown.IsActive || character.GlobalCooldown.IsActive;
            cooldownGroup.SetActive(isOnCooldown);
            if (!isOnCooldown) return;

            bool shouldDisplayGlobal = character.GlobalCooldown.RemainingTime > skill.Cooldown.RemainingTime;
            cooldownSlider.value = shouldDisplayGlobal
                ? character.GlobalCooldown.NormalizedTime
                : skill.Cooldown.NormalizedTime;

            cooldownText.text = shouldDisplayGlobal
                ? character.GlobalCooldown.RemainingTime.ToString("F1")
                : skill.Cooldown.RemainingTime.ToString("F1");

            fillImage.color = cooldownText.color = shouldDisplayGlobal ? globalCooldownColor : skillCooldownColor;
        }
    }
}