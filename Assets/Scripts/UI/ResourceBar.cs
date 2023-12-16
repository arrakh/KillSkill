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
    
    public class ResourceBar : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Image fillImage;

        private bool isActive;
        private ICharacterResource assignedResource;
        private IResourceBarDisplay barDisplay;

        public bool IsActive => isActive;

        public void Assign(ICharacterResource resource, IResourceBarDisplay display)
        {
            assignedResource = resource;
            barDisplay = display;

            fillImage.color = display.DisplaySettings.barColor;
            Debug.Log($"WILL SET COLOR TO {display.DisplaySettings.barColor}", gameObject);
            
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
            var settings = barDisplay.DisplaySettings;
            var min = (float) settings.min;
            var max = (float) settings.max;

            slider.value = Mathf.Clamp01(Mathf.InverseLerp(min, max, (float) newVal));
            fillImage.color = settings.barColor;
        }
    }
}