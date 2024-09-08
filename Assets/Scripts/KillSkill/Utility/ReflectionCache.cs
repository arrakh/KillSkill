using System;
using System.Collections.Generic;

namespace KillSkill.Utility
{
    public static class ReflectionCache
    {
        private static Dictionary<Type, IEnumerable<Type>> _typeCache = new();

        public static IEnumerable<Type> GetAll<T>(bool includeSelf = false)
        {
            var type = typeof(T);
            if (_typeCache.TryGetValue(type, out var cache)) return cache;
            var list = ReflectionUtility.GetAll<T>(includeSelf);
            _typeCache[type] = list;
            return list;
        }
    }
}