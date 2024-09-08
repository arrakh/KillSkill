using DG.Tweening;
using KillSkill.VisualEffects;
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

        private IEffectPool effectPool;
        private UnityEffect effect;

        public void Initialize(UnityEffect f, IEffectPool pool)
        {
            effect = f;
            effectPool = pool;
        }
        
        public void Display(string text, float duration, Color color)
        {
            flyingText.text = text;
            flyingText.color = color;
            visualTransform.localPosition = Vector3.zero;
            visualTransform.DOLocalMove(flyDirection, duration).SetEase(flyCurve).OnComplete(OnTweenComplete);
            flyingText.DOFade(0f, duration).SetEase(fadeCurve);
        }

        private void OnTweenComplete()
        {
            effectPool.Return(effect);
        }
    }
}