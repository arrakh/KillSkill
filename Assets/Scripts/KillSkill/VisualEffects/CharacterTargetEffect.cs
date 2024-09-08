using DG.Tweening;
using KillSkill.Characters;
using UnityEngine;
using UnityEngine.UI;

namespace KillSkill.VisualEffects
{
    public class CharacterTargetEffect : MonoBehaviour
    {
        [SerializeField] private Character owner;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Image mainBarGlow;
        [SerializeField] private Sprite redGlow, greenGlow, yellowGlow;
        private static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");

        public bool IsLockedOn => isLockedOn;
        public bool IsHighlighted => isHighlighted;
        private bool isHighlighted = false;
        private bool isLockedOn = false;

        private Tween glowScaleTween;

        public void SetStatus(bool lockedOn, bool highlighted )
        {
            isLockedOn = lockedOn;
            isHighlighted = highlighted;

            var characterColor = owner.IsEnemy ? Color.red : Color.green;
            var outlineColor = isHighlighted ? Color.yellow : isLockedOn ? characterColor : Color.black;
            spriteRenderer.material.SetColor(OutlineColor, outlineColor);
    
            mainBarGlow.gameObject.SetActive(lockedOn);
            
            if (lockedOn)
            {
                mainBarGlow.sprite = owner.IsEnemy ? redGlow : greenGlow;
            }
        }

        public void AnimateGlow()
        {
            if (glowScaleTween != null) DOTween.Kill(glowScaleTween);
            
            mainBarGlow.transform.localScale = new Vector3(1.1f, 1.5f, 1f);
            glowScaleTween = mainBarGlow.transform.DOScale(1f, 0.33f).SetEase(Ease.OutCirc);
        }
    }
}