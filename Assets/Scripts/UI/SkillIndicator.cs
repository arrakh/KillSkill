using System;
using Actors;
using Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SkillIndicator : MonoBehaviour
    {
        [SerializeField] private Character character;
        [SerializeField] private int skillIndex;
        [SerializeField] private GameObject cooldownGroup;
        [SerializeField] private Slider cooldownSlider;
        [SerializeField] private TextMeshProUGUI cooldownText;
        [SerializeField] private TextMeshProUGUI skillNameText;

        private Skill skill;

        private void Start()
        {
            skill = character.GetSkill(skillIndex);
            skillNameText.text = skill.DisplayName;
        }

        private void Update()
        {
            cooldownGroup.SetActive(skill.IsOnCooldown);
            if (!skill.IsOnCooldown) return;
            
            cooldownSlider.value = skill.NormalizedCooldown;
            cooldownText.text = skill.RemainingCooldownTime.ToString("F1");
        }
    }
}