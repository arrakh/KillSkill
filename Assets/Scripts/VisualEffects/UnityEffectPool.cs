using System;
using UnityEngine;
using UnityEngine.Pool;

namespace VisualEffects
{
    public class UnityEffectPool : IDisposable
    {
        private UnityEffectData data;
        private ObjectPool<UnityEffect> pool;
        private GameObject poolRoot;

        public UnityEffectPool(string key)
        {
            poolRoot = new GameObject($"unity-effect-pool-{key}");
            poolRoot.hideFlags = HideFlags.HideInHierarchy;
            
            data = UnityEffectDatabase.Get(key);
            pool = new ObjectPool<UnityEffect>(Create, Get, Release, Destroy, true, data.initialSize);
        }
        
        private UnityEffect Create()
        {
            var go = UnityEngine.Object.Instantiate(data.prefab);
            if (!go.TryGetComponent<UnityEffect>(out var component))
                component = go.AddComponent<UnityEffect>();
            
            component.Initialize();
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

        public UnityEffect Get() => pool.Get();

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