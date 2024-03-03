using System;
using System.Collections.Generic;
using CharacterResources;
using KillSkill.Characters;
using UnityEngine;

namespace UI
{
    public class CharacterResourcesDisplay : MonoBehaviour
    {
        [SerializeField] private CharacterResourceBar mainBar;
        [SerializeField] private List<CharacterResourceBar> bars;

        private Dictionary<Type, CharacterResourceBar> activeBars = new();

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
                mainBar.Assign(target, resource, barDisplay);
                return;
            }

            foreach (var bar in bars)
            {
                if (bar.IsActive) continue;
                activeBars[type] = bar;
                bar.Assign(target, resource, barDisplay);
                break;
            }
        }

        private void OnAnyUnassigned(Type type)
        {
            Debug.Log($"UNASSIGNING {type.Name}");
            if (!activeBars.Remove(type, out var bar)) return;
            bar.Unassign();
        }
    }
}