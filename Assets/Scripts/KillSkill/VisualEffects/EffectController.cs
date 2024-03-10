using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace VisualEffects
{
    public class EffectController : MonoBehaviour, IVisualEffectsHandler
    {
        private Dictionary<string, UnityEffectPool> unityEffects = new();

        public IEffect Spawn(string key, Vector3 position)
        {
            if (!unityEffects.TryGetValue(key, out var pool))
            {
                pool = new UnityEffectPool(key);
                unityEffects[key] = pool;
            }
            
            var obj = pool.Get();
            position.z -= 0.3f;
            obj.SetPosition(position);

            return obj;
        }
    }
}