using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arr.SDS
{
    public class PairScriptableDatabase<TKey, TValue> : BaseScriptableDatabase 
        where TKey : IEquatable<TKey> 
        where TValue : IScriptableKey<TKey>
    {
        [SerializeField] protected TValue[] data;

        public static Dictionary<TKey, TValue> _dict = new();

        public override void Initialize()
        {
            foreach (var d in data)
            {
                if (_dict.ContainsKey(d.Key)) throw new Exception($"Duplicate Data exist under {name}");
                _dict[d.Key] = d;
            }
        }

        public static TValue Get(TKey id)
        {
            if (_dict.TryGetValue(id, out var data)) return data;
            throw new Exception($"Trying to get data {id} but it does not exist or not initialized!");
        }

        public static bool TryGet(TKey id, out TValue data) => _dict.TryGetValue(id, out data);

        public static TValue[] GetAll() => _dict.Values.ToArray();
    }
}