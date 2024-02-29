using System;
using Actors;
using CharacterResources;
using CharacterResources.Implementations;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ResourceBarDisplaySettings
    {
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
        private ResourceBarDisplaySettings displaySettings;

        public bool IsActive => isActive;

        public void Assign(Character target, ICharacterResource resource, IResourceBarDisplay display)
        {
            assignedResource = resource;
            barDisplay = display;
            displaySettings = display.GetDisplaySettings(target);

            fillImage.color = displaySettings.barColor;
            Debug.Log($"WILL SET COLOR TO {displaySettings.barColor}", gameObject);
            
            resource.OnUpdated += OnResourceUpdated;
            gameObject.SetActive(true);
            isActive = true;
        }

        public void Unassign()
        {
            gameObject.SetActive(false);
            assignedResource.OnUpdated -= OnResourceUpdated;
            isActive = false;
        }

        private void OnResourceUpdated(double oldVal, double newVal)
        {
            var min = (float) displaySettings.min;
            var max = (float) displaySettings.max;

            slider.value = Mathf.Clamp01(Mathf.InverseLerp(min, max, (float) newVal));
            fillImage.color = displaySettings.barColor;
        }
    }
}