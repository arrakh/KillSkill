using Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KillSkill.UI
{
    public class PlayerResourceCounter : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI counterText;

        public void Display(string resourceId, double amount)
        {
            //icon.sprite = SpriteDatabase.Get($"resource-icon-{resourceId}");
            counterText.text = amount.ToString();
        }
    }
}