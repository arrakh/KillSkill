using TMPro;
using UnityEngine;

namespace KillSkill.UI.Battle.GameResult
{
    public class GameResultRewardElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI conditionText;
        [SerializeField] private ResourceElement resourceCounter;

        public void Display(string condition, string resource, double value)
        {
            conditionText.text = condition;
            resourceCounter.Display(resource, value);
        }
    }
}