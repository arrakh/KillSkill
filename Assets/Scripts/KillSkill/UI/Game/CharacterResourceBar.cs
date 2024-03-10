using CharacterResources;
using KillSkill.CharacterResources;
using KillSkill.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KillSkill.UI.Game
{
    public class ResourceBarDisplay
    {
        public double value;
        public double min;
        public double max;
        public bool showValueText = true;
        public Color barColor;
    }
    
    public class CharacterResourceBar : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Image fillImage;
        [SerializeField] private TextMeshProUGUI valueText;

        private bool isActive;
        private IResourceDisplay<ResourceBarDisplay> barDisplay;

        public bool IsActive => isActive;

        public void Assign(Character target, IResourceDisplay<ResourceBarDisplay> display)
        {
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
            valueText.text = display.showValueText ? $"{display.value} / {max}" : "";
        }
    }
}