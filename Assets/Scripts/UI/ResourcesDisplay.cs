using System;
using System.Collections.Generic;
using Actors;
using CharacterResources;
using UnityEngine;

namespace UI
{
    public class ResourcesDisplay : MonoBehaviour
    {
        [SerializeField] private ResourceBar mainBar;
        [SerializeField] private List<ResourceBar> bars;

        private Dictionary<Type, ResourceBar> activeBars = new();

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
            //TODO: CREATE COUNTERS

            if (resource is not IResourceBarDisplay barDisplay) return;

            if (type == target.MainResource)
            {
                mainBar.Assign(resource, barDisplay);
                return;
            }

            foreach (var bar in bars)
            {
                if (bar.IsActive) continue;
                activeBars[type] = bar;
                bar.Assign(resource, barDisplay);
                break;
            }
        }

        private void OnAnyUnassigned(Type type)
        {
            if (!activeBars.Remove(type, out var bar)) return;
            bar.Unassign();
        }
    }
}