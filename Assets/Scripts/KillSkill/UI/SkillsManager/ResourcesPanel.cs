using System.Collections.Generic;
using SessionData.Implementations;
using UnityEngine;

namespace KillSkill.UI.SkillsManager
{
    public class ResourcesPanel : MonoBehaviour
    {
        [SerializeField] private GameObject counterPrefab;
        [SerializeField] private RectTransform elementParent;

        private List<PlayerResourceCounter> spawnedCounters = new();

        public void Display(IReadOnlyDictionary<string, double> resources)
        {
            CleanElements();
            
            foreach (var resource in resources)
            {
                var obj = Instantiate(counterPrefab, elementParent);
                var element = obj.GetComponent<PlayerResourceCounter>();
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
    }
}