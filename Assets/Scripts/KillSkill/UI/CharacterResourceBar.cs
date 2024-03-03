using System;
using CharacterResources;
using KillSkill.Characters;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ResourceBarDisplay
    {
        public double value;
        public double min;
        public double max;
        public Color barColor;
    }
    
    public class CharacterResourceBar : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Image fillImage;

        private bool isActive;
        private ICharacterResource assignedResource;
        private IResourceBarDisplay barDisplay;

        public bool IsActive => isActive;

        public void Assign(Character target, ICharacterResource resource, IResourceBarDisplay display)
        {
            assignedResource = resource;
            barDisplay = display;

            OnDisplayUpdated(display.DisplayData);
            
            display.OnUpdateDisplay += OnDisplayUpdated;
            gameObject.SetActive(true);
            isActive = true;
        }

        public void Unassign()
        {
            gameObject.SetActive(false);
            barDisplay.OnUpdateDisplay -= OnDisplayUpdated;
            isActive = false;
        }

        private void OnDisplayUpdated(ResourceBarDisplay display)
        {
            var min = (float) display.min;
            var max = (float) display.max;

            slider.value = Mathf.Clamp01(Mathf.InverseLerp(min, max, (float) display.value));
            fillImage.color = display.barColor;
        }
    }
}