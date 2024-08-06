using Arr.EventsSystem;
using Arr.ViewModuleSystem;
using DG.Tweening;
using KillSkill.Battle;
using KillSkill.Modules.Loaders.Events;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace KillSkill.UI.Battle.GameResult
{
    public class ResultView : View
    {
        [SerializeField] private RectTransform panel;
        [SerializeField] private TextMeshProUGUI resultText;
        [SerializeField] private GameObject rewardPrefab, statPrefab;
        [SerializeField] private RectTransform rewardParent, statParent;
        [SerializeField] private float inAnimTime;
        [SerializeField] private AnimationCurve inAnimCurve;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Button retryButton, exitButton;

        private void Awake()
        {
            retryButton.onClick.AddListener(OnRetry);
            exitButton.onClick.AddListener(OnExit);
        }

        private void OnExit()
        {
            GlobalEvents.Fire(new SwitchContextEvent(SwitchContextEvent.Type.Lobby));
        }

        private void OnRetry()
        {
            GlobalEvents.Fire(new SwitchContextEvent(SwitchContextEvent.Type.Battle));
        }

        public void Display(BattleResultData result)
        {
            InAnimation();

            resultText.text = result.playerWon ? "Victory" : "Defeat";

            foreach (var reward in result.rewards)
            {
                var element = Instantiate(rewardPrefab, rewardParent)
                    .GetComponent<GameResultRewardElement>();
                    
                element.Display(reward.resultText, reward.resourceId, reward.resourceAmount);
            }
        }

        private void InAnimation()
        {
            DOTween.Kill(gameObject);

            var panelY = panel.sizeDelta.y;
            panel.sizeDelta = new Vector2(panel.sizeDelta.x, 0f);
            panel.DOSizeDelta(new Vector2(panel.sizeDelta.x, panelY), inAnimTime).SetEase(inAnimCurve);
            canvasGroup.alpha = 0f;
            canvasGroup.DOFade(1f, inAnimTime);
        }
    }
}