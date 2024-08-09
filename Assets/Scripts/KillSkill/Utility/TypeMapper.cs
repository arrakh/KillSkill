using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KillSkill.Utility
{
    public class TypeMapper<T>
    {
        private Dictionary<uint, Type> toTypeMapping = new();
        private Dictionary<Type, uint> toIdMapping = new();

        private int lastIndex;

        public int LastIndex => lastIndex;
        
        public TypeMapper()
        {
            var types = ReflectionCache.GetAll<T>().OrderBy(t => t.FullName).ToArray();

            lastIndex = types.Length;
            for (uint i = 1; i < types.Length + 1; i++)
            {
                var type = types[i - 1];
                toTypeMapping[i] = type;
                toIdMapping[type] = i;
            }
        }

        public Type ToType(uint id) => toTypeMapping[id];
        public uint ToId(Type type) => toIdMapping[type];

        public Type[] ToTypeArray(uint[] ids)
        {
            Type[] arr = new Type[ids.Length];
            for (uint i = 0; i < ids.Length; i++)
                if (ids[i] == 0) arr[i] = null;
                else arr[i] = toTypeMapping[ids[i]];
            return arr;
        }

        public uint[] ToIdArray(Type[] types)
        {
            uint[] arr = new uint[types.Length];
            for (uint i = 0; i < types.Length; i++)
                if (types[i].IsEmpty()) arr[i] = 0;
                else arr[i] = toIdMapping[types[i]];
            return arr;
        }
    }
}