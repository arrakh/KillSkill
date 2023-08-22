using System;
using Actors;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Character character;
        [SerializeField] private Slider healthSlider;
        
        private void OnEnable()
        {
            healthSlider.value = character.Hp / character.MaxHp;
            character.OnHealthChanged += OnHealthChanged;
        }

        private void OnDisable()
        {
            character.OnHealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(float oldValue, float newValue)
        {
            healthSlider.value = newValue / character.MaxHp;
        }
    }
}