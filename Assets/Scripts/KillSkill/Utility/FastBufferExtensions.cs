using System;
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
    }
}