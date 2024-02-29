using System;
using System.Collections.Generic;
using UnityEngine;
using VisualEffects.EffectComponents;

namespace VisualEffects
{
    public class UnityEffect : MonoBehaviour, IEffect
    {
        private Dictionary<Type, IEffectComponent> effectComponents = new();

        private bool shouldFollow = false;
        private Transform follow = null;

        private void Update()
        {
            if (!shouldFollow) return;
            transform.position = follow.position;
            transform.rotation = follow.rotation;
            transform.localScale = follow.localScale;
        }

        public void Initialize()
        {
            foreach (var component in GetComponents<Component>())
                if (component is IEffectComponent effect)
                    effectComponents[effect.GetType()] = effect;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void Follow(Transform toFollow)
        {
            follow = toFollow;
            shouldFollow = true;
        }

        public void Unfollow()
        {
            shouldFollow = false;
            follow = null;
        }

        public T GetEffectComponent<T>() where T : IEffectComponent
        {
            if (effectComponents[typeof(T)] is T t) return t;
            throw new ArgumentOutOfRangeException($"Type {typeof(T)} does not exist!");
        }

        public bool TryGetEffectComponent<T>(out T component) where T : IEffectComponent
        {
            component = default;
            if (!effectComponents.TryGetValue(typeof(T), out var effectComponent)) return false;
            if (effectComponent is not T t) return false;
            component = t;
            return true;
        }
    }
}