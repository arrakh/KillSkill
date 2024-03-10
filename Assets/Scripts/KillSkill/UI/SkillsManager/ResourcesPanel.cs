using System.Collections.Generic;
using DG.Tweening;
using SessionData.Implementations;
using TMPro;
using UnityEngine;

namespace KillSkill.UI.SkillsManager
{
    public class ResourcesPanel : MonoBehaviour
    {
        [SerializeField] private GameObject counterPrefab;
        [SerializeField] private RectTransform elementParent;
        [SerializeField] private TextMeshProUGUI titleText;

        private List<ResourceElement> spawnedCounters = new();

        public void Display(IReadOnlyDictionary<string, double> resources)
        {
            CleanElements();
            
            foreach (var resource in resources)
            {
                var obj = Instantiate(counterPrefab, elementParent);
                var element = obj.GetComponent<ResourceElement>();
                element.Display(resource.Key, resource.Value);
                spawnedCounters.Add(element);
            }
        }

        private void CleanElements()
        {
            foreach (var counter in spawnedCounters)
                Destroy(counter.gameObject);
            
            spawnedCounters.Clear();
        }

        public void AnimateCannotBuy()
        {
            DOTween.Kill(titleText.gameObject);
            DOTween.Kill(elementParent.gameObject);
            elementParent.DOShakePosition(0.3f, Vector3.right * 10f);
            titleText.color = Color.red;
            
            titleText.DOColor(Color.white, 0.3f);
        }
    }
}