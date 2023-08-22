using System;
using Actors;
using TMPro;
using UnityEngine;

namespace UI
{
    public class StatusEffectIndicator : MonoBehaviour
    {
        [SerializeField] private Character character;
        [SerializeField] private TextMeshProUGUI statusText;

        private void Update()
        {
            var status = string.Empty;

            foreach (var statusEffect in character.GetStatusEffects())
            {
                status += statusEffect.DisplayName + "... " + statusEffect.RemainingDuration.ToString("F1") + "\n";
            }

            statusText.text = status;
        }
    }
}