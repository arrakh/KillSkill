using System;
using System.Collections.Generic;
using CharacterResources;
using KillSkill.CharacterResources;
using KillSkill.Characters;
using KillSkill.UI.Game;
using Unity.VisualScripting;
using UnityEngine;

namespace KillSkill.UI.Battle
{
    public class CharacterResourcesDisplay : MonoBehaviour
    {
        [SerializeField] private CharacterResourceBar mainBar;
        [SerializeField] private List<CharacterResourceBar> bars;
        [SerializeField] private RectTransform counterGridParent;
        [SerializeField] private GameObject counterPrefab;

        private Dictionary<Type, CharacterResourceBar> activeBars = new();
        private Dictionary<Type, GameObject> activeCounters = new();

        private ICharacter target;

        public void Initialize(ICharacter character)
        {
            character.OnInitialize.Subscribe(OnCharacterInitialized);
        }

        private void OnCharacterInitialized(ICharacter t)
        {
            target = t;
            target.Resources.ObserveAnyAssigned(OnAnyAssigned);
            target.Resources.ObserveAnyUnassigned(OnAnyUnassigned);
        }

        private void OnAnyAssigned(Type type, ICharacterResource resource)
        {
            if (resource is IResourceDisplay<ResourceBarDisplay> barDisplay) DisplayBar(type, barDisplay);
            else if (resource is IResourceDisplay<ResourceCounterDisplay> counter) DisplayCounter(type, counter);
        }

        private void DisplayCounter(Type type, IResourceDisplay<ResourceCounterDisplay> counterDisplay)
        {
            var obj = Instantiate(counterPrefab, counterGridParent);
            var counter = obj.GetComponent<CharacterResourceCounter>();
            counter.Assign(counterDisplay);
            activeCounters[type] = obj;
        }

        private void DisplayBar(Type type, IResourceDisplay<ResourceBarDisplay> barDisplay)
        {
            if (type == target.MainResource)
            {
                mainBar.Assign(target, barDisplay);
                return;
            }

            foreach (var bar in bars)
            {
                if (bar.IsActive) continue;
                activeBars[type] = bar;
                bar.Assign(target, barDisplay);
                break;
            }
        }

        private void OnAnyUnassigned(Type type)
        {
            if (activeCounters.Remove(type, out var counter)) Destroy(counter);
            if (activeBars.Remove(type, out var bar)) bar.Unassign();
        }
    }
}