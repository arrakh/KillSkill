using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace KillSkill.Network
{
    public class MessageSizeCache
    {
        private Dictionary<Type, int> sizeCache = new();
        
        public int GetSize(INetCodeSerializable message)
        {
            var type = message.GetType();
            if (sizeCache.TryGetValue(type, out var size)) return size;

            var packetSize = 1024;
            bool sizeFound = false;

            do
            {
                using var writer = new FastBufferWriter(sizeof(ushort) + packetSize, Allocator.Temp);
                if (writer.TryBeginWrite(sizeof(ushort) + packetSize))
                {
                    message.Serialize(writer);
                    size = writer.Position;
                    sizeCache[type] = size;
                    Debug.Log($"[MSC] SIZE FOUND! {size}");
                    sizeFound = true;
                }
                else packetSize *= 2;
            } 
            while (!sizeFound);

            return size;
        }
    }
}