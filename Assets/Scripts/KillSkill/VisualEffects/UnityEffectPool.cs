using System;
using UnityEngine;
using UnityEngine.Pool;
using VisualEffects;

namespace KillSkill.VisualEffects
{
    public class UnityEffectPool : IDisposable, IEffectPool
    {
        private UnityEffectData data;
        private ObjectPool<UnityEffect> pool;
        private GameObject poolRoot;

        public UnityEffectPool(string key)
        {
            poolRoot = new GameObject($"unity-effect-pool-{key}");
            
            data = UnityEffectDatabase.Get(key);
            pool = new ObjectPool<UnityEffect>(Create, Get, Release, Destroy, true, data.initialSize, data.maxSize);
        }
        
        private UnityEffect Create()
        {
            var go = UnityEngine.Object.Instantiate(data.prefab, poolRoot.transform);
            if (!go.TryGetComponent<UnityEffect>(out var component))
                component = go.AddComponent<UnityEffect>();
            
            component.Initialize(this, data.timeoutDuration);
            go.SetActive(false);
            return component;
        }

        private void Get(UnityEffect obj)
        {
            obj.gameObject.SetActive(true);
        }

        private void Release(UnityEffect obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void Destroy(UnityEffect obj)
        {
            
        }

        public UnityEffect Get()
        {
            var effect = pool.Get();
            effect.ResetEffect();
            return effect;
        }

        public void Return(UnityEffect effect)
        {
            effect.Kill();
            pool.Release(effect);
        }

        private bool disposed = false;
        public void Dispose()
        {
            if (disposed) return;
            pool?.Dispose();
            UnityEngine.Object.DestroyImmediate(poolRoot);

            disposed = true;
        }
    }
}