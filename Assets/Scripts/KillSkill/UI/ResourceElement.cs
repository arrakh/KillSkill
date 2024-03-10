using KillSkill.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KillSkill.UI
{
    public class ResourceElement : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI counterText;

        public void Display(string resourceId, double amount)
        {
            icon.sprite = SpriteDatabase.Get(resourceId);
            counterText.text = amount.ToString();
        }
    }
}