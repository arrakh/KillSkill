using System;
using Arr.ScriptableDatabases;
using UnityEngine;

namespace VisualEffects
{
    [Serializable]
    public class UnityEffectData : IScriptableKey<string>
    {
        public string id;
        public GameObject prefab;
        public int initialSize;
        public int maxSize;
        public float timeoutDuration;
        public string Key => id;
    }
    
    [CreateAssetMenu(menuName = "Unity Effect Database", fileName = "Unity Effect Database")]
    public class UnityEffectDatabase : PairScriptableDatabase<string, UnityEffectData>
    {
        public override void Initialize()
        {
            base.Initialize();

            /*foreach (var key in _dict)
            {
                Debug.Log($"UNITY EFFECT {key.Key}");
            }*/
        }
    }
}