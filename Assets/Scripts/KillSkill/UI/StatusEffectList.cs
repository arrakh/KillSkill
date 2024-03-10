using System;
using System.Collections.Generic;
using KillSkill.Characters;
using KillSkill.StatusEffects;
using KillSkill.UI;
using StatusEffects;
using TMPro;
using UnityEngine;

namespace UI
{
    public class StatusEffectList : MonoBehaviour
    {
        [SerializeField] private Character character;
        [SerializeField] private RectTransform parent;
        [SerializeField] private GameObject elementPrefab;
        
        private Dictionary<Type, StatusEffectListElement> spawnedElements = new();

        private void OnEnable()
        {
            character.onInitialize.Subscribe(OnInitialized);
        }

        private void OnInitialized(Character c)
        {
            c.onInitialize.Unsubscribe(OnInitialized);

            c.StatusEffects.OnAdded += OnStatusEffectAdded;
            c.StatusEffects.OnRemoved += OnStatusEffectRemoved;
        }

        private void OnStatusEffectAdded(IStatusEffect effect)
        {
            var type = effect.GetType();
            if (spawnedElements.ContainsKey(type)) return;

            var element = Instantiate(elementPrefab, parent, false).GetComponent<StatusEffectListElement>();
            spawnedElements[type] = element;
            element.Display(effect);
        }
        
        private void OnStatusEffectRemoved(IStatusEffect effect)
        {
            var type = effect.GetType();
            if (!spawnedElements.TryGetValue(type, out var element)) return;
            
            Destroy(element.gameObject);

            spawnedElements.Remove(type);
        }
    }
}