using Arr;
using Arr.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CountdownUI : MonoBehaviour
    {
        [SerializeField] private GameObject visibilityGroup;
        [SerializeField] private Image bg;
        [SerializeField] private TextMeshProUGUI countText;
        [SerializeField] private float textAnimDuration, textAnimScale;
        [SerializeField] private float bgHeightFrom, bgHeightTo, bgAnimDuration;
        [SerializeField] private float lastCountDuration;

        private int scaleId, fadeId;
        
        public void Count(int counter)
        {
            if (!visibilityGroup.activeInHierarchy) visibilityGroup.SetActive(true);

            DOTween.Kill(scaleId);
            DOTween.Kill(fadeId);
            
            bool lastCount = counter <= 0;
            countText.text = lastCount ? "FIGHT" : counter.ToString();

            var textDuration = lastCount ? lastCountDuration : textAnimDuration;
            
            countText.transform.localScale = Vector3.one;
            scaleId = countText.transform.DOScale(Vector3.one * textAnimScale, textDuration).intId;
            
            countText.color = countText.color.Alpha(1f);
            fadeId = countText.DOFade(0f, textDuration).intId;
            if (!lastCount) return;

            var size = bg.rectTransform.sizeDelta;
            size.y = bgHeightFrom;
            bg.rectTransform.sizeDelta = size;

            bg.rectTransform.DOSizeDelta(new Vector2(size.x, bgHeightTo), bgAnimDuration).OnComplete(() =>
            {
                visibilityGroup.SetActive(false);
            });

            bg.DOFade(0f, bgAnimDuration);
        }
    }
}