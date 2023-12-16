using System;
using System.Collections.Generic;
using Arr.SDS;
using UnityEngine;

namespace Arr.PrefabRegistrySystem
{
    [Serializable]
    public struct PrefabRegistryEntry
    {
        public string id;
        public GameObject prefab;
    }
    
    [CreateAssetMenu(menuName = "Prefab Registry")]
    public class PrefabRegistry : BaseScriptableDatabase
    {
        private static Dictionary<string, GameObject> prefabs = new();

        public PrefabRegistryEntry[] entries;
        
        public override void Initialize()
        {
            foreach (var entry in entries)
            {
                if (prefabs.ContainsKey(entry.id))
                    throw new Exception($"Trying to add prefabs to registry but {entry.id} is a duplicate that exists on {name}");

                prefabs[entry.id] = entry.prefab;
            } 
        }

        public static GameObject Get(string id) => prefabs[id];
        public static bool TryGet(string id, out GameObject prefab) => prefabs.TryGetValue(id, out prefab);
    }
}