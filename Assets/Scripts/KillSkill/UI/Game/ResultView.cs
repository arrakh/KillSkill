using System;
using Arr.ViewModuleSystem;
using DefaultNamespace;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KillSkill.UI.Game
{
    public class ResultView : View
    {
        [SerializeField] private RectTransform panel;
        [SerializeField] private TextMeshProUGUI resultText;
        [SerializeField] private GameObject resourcePrefab;
        [SerializeField] private RectTransform resourceParent;
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
            
        }

        private void OnRetry()
        {
            
        }

        public void Display(BattleResultData result)
        {
            InAnimation();

            resultText.text = result.playerWon ? "Victory" : "Defeat";

            foreach (var resource in result.gainedResources)
            {
                var counter = Instantiate(resourcePrefab, resourceParent)
                    .GetComponent<PlayerResourceCounter>();
                    
                counter.Display(resource.Key, resource.Value);
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