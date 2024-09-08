using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KillSkill.Network;
using Unity.Netcode;

namespace KillSkill.Utility
{
    public static class FastBufferExtensions
    {
        public static void Read<T>(this FastBufferReader reader, out T data) where T : INetCodeSerializable, new()
        {
            data = new T();
            data.Deserialize(reader);
        }

        public static void Write<T>(this FastBufferWriter writer, T data) where T : INetCodeSerializable
        {
            data.Serialize(writer);
        }

        public static void WriteValueSafe<T>(this FastBufferWriter writer, ICollection<T> enumerable)
            where T : INetCodeSerializable
        {
            var count = enumerable.Count;
            writer.WriteValueSafe(count);
            foreach (var item in enumerable)
                item.Serialize(writer);
        }

        public static void ReadValueSafe<T>(this FastBufferReader reader, out ICollection<T> collection)
            where T : INetCodeSerializable, new()
        {
            reader.ReadValueSafe(out int count);
            var array = new T[count];
            for (int i = 0; i < count; i++)
            {
                var data = new T();
                data.Deserialize(reader);
                array[i] = data;
            }

            collection = array;
        }

        public static void WriteValueSafe(this FastBufferWriter writer, ICollection<string> stringCollection)
        {
            var count = stringCollection.Count;
            writer.WriteValueSafe(count);
            foreach (var item in stringCollection)
                writer.WriteValueSafe(item);
        }

        public static void ReadValueSafe(this FastBufferReader reader, out ICollection<string> collection)
        {
            reader.ReadValueSafe(out int count);
            var array = new string[count];
            for (int i = 0; i < count; i++)
                reader.ReadValueSafe(out array[i]);

            collection = array;
        }
    }
}