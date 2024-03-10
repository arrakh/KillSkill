using System;
using CharacterResources;
using KillSkill.CharacterResources;
using KillSkill.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KillSkill.UI.Game
{
    public class ResourceFillCounterDisplay
    {
        public int value = 0;
        public int maxValue = 0;
        public float fillValue;
        public Sprite icon;
    }
    
    public class CharacterResourceFillCounter : MonoBehaviour
    {
        [SerializeField] private Image fillImage, icon;
        [SerializeField] private TextMeshProUGUI valueText;
        
        private IResourceDisplay<ResourceFillCounterDisplay> barDisplay;


        public void Assign(IResourceDisplay<ResourceFillCounterDisplay> display)
        {
            barDisplay = display;

            OnDisplayUpdated(display.DisplayData);
            
            display.OnUpdateDisplay += OnDisplayUpdated;
        }

        private void OnDestroy()
        {
            barDisplay.OnUpdateDisplay -= OnDisplayUpdated;
        }

        private void OnDisplayUpdated(ResourceFillCounterDisplay display)
        {
            fillImage.fillAmount = Mathf.Clamp01(display.fillValue);
            icon.sprite = display.icon;
            var hasMax = display.maxValue > 0;
            valueText.text = hasMax ? $"{display.value} / {display.maxValue}" : $"{display.value}";
        }
    }
}