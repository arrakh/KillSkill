using DG.Tweening;
using TMPro;
using UnityEngine;

namespace VisualEffects.EffectComponents
{
    public class FlyingTextComponent : MonoBehaviour, IEffectComponent
    {
        [SerializeField] private TextMeshPro flyingText;
        [SerializeField] private Transform visualTransform;
        [SerializeField] private Vector2 flyDirection;
        [SerializeField] private AnimationCurve flyCurve;
        [SerializeField] private AnimationCurve fadeCurve;

        public void Display(string text, float duration, Color color)
        {
            flyingText.text = text;
            flyingText.color = color;
            visualTransform.localPosition = Vector3.zero;
            visualTransform.DOLocalMove(flyDirection, duration).SetEase(flyCurve);
            flyingText.DOFade(0f, duration).SetEase(fadeCurve);
        }
    }
}