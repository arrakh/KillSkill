using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KillSkill.UI
{
    public class StatusEffectNameplate : MonoBehaviour
    {
        [SerializeField] private float duration;
        [SerializeField] private float flashDuration = 0.15f;
        [SerializeField] private AnimationCurve animation;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Image stabFlash;
        [SerializeField] private TextMeshProUGUI name;

        public void Display(string statusName)
        {
            name.text = statusName;

            DOTween.Kill(gameObject);

            canvasGroup.alpha = 1f;
            canvasGroup.DOFade(0f, duration).SetEase(animation);
            
            stabFlash.color = Color.white;
            stabFlash.DOFade(0f, flashDuration);
        }
    }
}