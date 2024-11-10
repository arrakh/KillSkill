using System;
using System.Collections.Generic;
using KillSkill.VisualEffects;
using UnityEngine;
using VisualEffects.EffectComponents;

namespace VisualEffects
{
    public class UnityEffect : MonoBehaviour, IEffect
    {
        private Dictionary<Type, IEffectComponent> effectComponents = new();
        private IEffectPool pool;
        private bool shouldFollow = false;
        private Transform follow = null;
        private float timeout = 0f;
        private float currentTimeoutTimer = 0f;
        private bool isAlive = false;

        private void Update()
        {
            TimeoutUpdate();
            if (!shouldFollow) return;
            transform.position = follow.position;
            transform.rotation = follow.rotation;
            transform.localScale = follow.localScale;
        }

        private void TimeoutUpdate()
        {
            if (timeout <= 0f) return;

            if (!isAlive) return;

            currentTimeoutTimer += Time.deltaTime;
            if (currentTimeoutTimer < timeout) return;

            isAlive = false;
            pool.Return(this);
        }

        public void ResetEffect()
        {
            currentTimeoutTimer = 0f;
            isAlive = true;
        }

        public void Kill()
        {
            isAlive = false;
        }

        public void Initialize(IEffectPool effectPool, float maxTimeout)
        {
            pool = effectPool;
            foreach (var component in GetComponents<Component>())
                if (component is IEffectComponent effect)
                {
                    effectComponents[effect.GetType()] = effect;
                    effect.Initialize(this, pool);
                }

            timeout = maxTimeout;
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

        public void Stop()
        {
            Kill();
            pool.Return(this);
        }
    }
}