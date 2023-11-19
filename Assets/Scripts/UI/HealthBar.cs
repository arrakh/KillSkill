using System;
using Actors;
using CharacterResources.Implementations;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Character character;
        [SerializeField] private Slider healthSlider;

        private Health health;
        
        private void OnEnable()
        {
            character.Resources.ObserveAssigned<Health>(OnHealthAssigned);
        }

        private void OnHealthAssigned(Health assignedHealth)
        {
            health = assignedHealth;
            health.OnHealthChanged += OnHealthChanged;
        }

        private void OnHealthChanged(double oldValue, double newValue)
        {
            healthSlider.value = (float) (newValue / health.Max);
        }
    }
}