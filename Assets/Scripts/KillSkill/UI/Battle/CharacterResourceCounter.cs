using KillSkill.CharacterResources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KillSkill.UI.Battle
{
    public class ResourceCounterDisplay
    {
        public int value = 0;
        public Sprite icon;
    }
    
    public class CharacterResourceCounter : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI valueText;
        
        private IResourceDisplay<ResourceCounterDisplay> barDisplay;


        public void Assign(IResourceDisplay<ResourceCounterDisplay> display)
        {
            barDisplay = display;

            OnDisplayUpdated(display.DisplayData);
            
            display.OnUpdateDisplay += OnDisplayUpdated;
        }

        private void OnDestroy()
        {
            barDisplay.OnUpdateDisplay -= OnDisplayUpdated;
        }

        private void OnDisplayUpdated(ResourceCounterDisplay display)
        {
            icon.sprite = display.icon;
            valueText.text = display.value.ToString();
        }
    }
}