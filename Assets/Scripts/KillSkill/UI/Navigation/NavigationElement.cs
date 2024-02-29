using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KillSkill.UI.Navigation
{
    public class NavigationElement : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image inverseBg;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Color normalTextColor, highlightTextColor;
        [SerializeField] private float animTime = 0.25f;
        [SerializeField] private AnimationCurve animCurve;

        private Action<NavigationElement> onNavClick;
        private INavigateSection currentSection;
        
        public INavigateSection Section => currentSection;

        private bool isHighlighted = false;

        private void Awake()
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            onNavClick?.Invoke(this);
        }

        public void Display(INavigateSection section, Action<NavigationElement> onClick)
        {
            currentSection = section;
            onNavClick = onClick;
            nameText.text = section.Name;
        }
        
        public void SetHighlight(bool isHighlight)
        {
            if (isHighlight == isHighlighted) return;
            isHighlighted = isHighlight;
            
            DOTween.Kill(this);
            
            var color = isHighlight ? highlightTextColor : normalTextColor;
            nameText.DOColor(color, animTime).SetEase(animCurve);

            var alpha = isHighlight ? 1f : 0f;
            inverseBg.DOFade(alpha, animTime).SetEase(animCurve);
        }
    }
}