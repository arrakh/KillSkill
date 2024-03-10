using TMPro;
using UnityEngine;

namespace KillSkill.UI.Game.GameResult
{
    public class GameResultStatElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI statName;
        [SerializeField] private TextMeshProUGUI statValue;

        public void Display(string stat, string value)
        {
            statName.text = stat;
            statValue.text = value;
        }
    }
}