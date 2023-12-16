using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Arr.SDS
{
    public class ObjectScriptableDatabase<T> : BaseScriptableDatabase where T : Object
    {
        [SerializeField] protected List<T> data = new();

        protected static Dictionary<string, T> _items = new ();

#if UNITY_EDITOR
        protected override bool CanFilter => true;
        protected override string Filter => $"t:{typeof(T).FullName}";
        
        protected override void OnPrePopulate() => data.Clear();

        protected override void OnPopulatePathFound(string path)
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            data.Add(asset);
        }
#endif

        public sealed override void Initialize()
        {
            foreach (var item in data)
            {
                string id = item.name;
                if (item is IScriptableKey scriptableData) id = scriptableData.Id;
                
                if (string.IsNullOrWhiteSpace(id)) 
                    throw new Exception($"Trying to Initialize {name} but {item.name} has empty id!");
                
                if (_items.ContainsKey(id))
                    throw new Exception($"Trying to Initialize {GetType()} but {name} has a duplicate id!");
                
                _items[id] = item;
                
                OnDataRegistered(item);
            }
            
            OnAllDataRegistered();
        }

        protected virtual void OnDataRegistered(T initializedData) {}
        
        protected virtual void OnAllDataRegistered(){}

        public static T Get(string id)
        {
            if (_items.TryGetValue(id, out var data)) return data;
            throw new Exception($"Trying to get data {id} but it does not exist or not initialized!");
        }

        public static bool TryGet(string id, out T data) => _items.TryGetValue(id, out data);

    }
}