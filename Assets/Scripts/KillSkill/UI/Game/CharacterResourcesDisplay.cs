using System;
using System.Collections.Generic;
using CharacterResources;
using KillSkill.CharacterResources;
using KillSkill.Characters;
using UnityEngine;

namespace KillSkill.UI.Game
{
    public class CharacterResourcesDisplay : MonoBehaviour
    {
        [SerializeField] private CharacterResourceBar mainBar;
        [SerializeField] private List<CharacterResourceBar> bars;
        [SerializeField] private RectTransform counterGridParent;
        [SerializeField] private GameObject fillCounterPrefab;
        [SerializeField] private GameObject counterPrefab;

        private Dictionary<Type, CharacterResourceBar> activeBars = new();
        private Dictionary<Type, GameObject> activeCounters = new();

        private Character target;

        public void Initialize(Character character)
        {
            character.onInitialize.Subscribe(OnCharacterInitialized);
        }

        private void OnCharacterInitialized(Character t)
        {
            target = t;
            target.Resources.ObserveAnyAssigned(OnAnyAssigned);
            target.Resources.ObserveAnyUnassigned(OnAnyUnassigned);
        }

        private void OnAnyAssigned(Type type, ICharacterResource resource)
        {
            if (resource is IResourceDisplay<ResourceBarDisplay> barDisplay) DisplayBar(type, barDisplay);
            else if (resource is IResourceDisplay<ResourceFillCounterDisplay> fillCounter) DisplayFillCounter(type, fillCounter);
        }

        private void DisplayFillCounter(Type type, IResourceDisplay<ResourceFillCounterDisplay> fillCounter)
        {
            var obj = Instantiate(fillCounterPrefab, counterGridParent);
            var counter = obj.GetComponent<CharacterResourceFillCounter>();
            counter.Assign(fillCounter);
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