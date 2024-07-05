using DG.Tweening;
using KillSkill.StatusEffects;
using UI.Tooltips;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace KillSkill.UI
{
    public class StatusEffectListElement : MonoBehaviour, ITooltipElement
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private Image fillImage;

        private IStatusEffect statusEffect;

        public void Display(IStatusEffect status)
        {
            statusEffect = status;
            var desc = status.Description;

            iconImage.sprite = desc.icon;

            Animate();
        }

        private void Animate()
        {
            DOTween.Kill(gameObject);
            
            transform.localScale = Vector3.one * 1.2f;
            transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutQuart);
        }

        public void Update()
        {
            if (statusEffect == null) return;
            
            if (statusEffect is ITimedStatusEffect timer)
            {
                fillImage.fillAmount = timer.NormalizedDuration;
                fillImage.enabled = true;
            }
            else fillImage.enabled = false;
        }

        public bool HasData() => statusEffect is {IsActive: true};

        public TooltipData GetData()
        {
            var desc = statusEffect.Description;
            return new TooltipData(desc.icon, desc.name, desc.description);
        }

        public int UniqueId => gameObject.GetInstanceID();
    }
}