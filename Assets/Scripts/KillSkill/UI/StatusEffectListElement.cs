using KillSkill.StatusEffects;
using StatusEffects;
using UI.Tooltips;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StatusEffectListElement : MonoBehaviour, ITooltipElement
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private Image fillImage;

        private IStatusEffect statusEffect;

        public void Initialize(IStatusEffect status)
        {
            statusEffect = status;
            var desc = status.Description;

            iconImage.sprite = desc.icon;
        }

        public void Update()
        {
            if (statusEffect == null) return;
            
            if (statusEffect is ITimerStatusEffect timer)
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