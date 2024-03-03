using System;
using System.Collections.Generic;
using KillSkill.Characters;
using KillSkill.StatusEffects;
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
            if (spawnedElements.ContainsKey(type))
                throw new Exception($"Trying to add status effect type {type} but element already exist??");

            var element = Instantiate(elementPrefab, parent, false).GetComponent<StatusEffectListElement>();
            spawnedElements[type] = element;
            element.Initialize(effect);
        }
        
        private void OnStatusEffectRemoved(IStatusEffect effect)
        {
            var type = effect.GetType();
            if (!spawnedElements.TryGetValue(type, out var element))
                throw new Exception($"Trying to remove status effect type {type} but element already does not exist??");
            
            Destroy(element.gameObject);

            spawnedElements.Remove(type);
        }
    }
}