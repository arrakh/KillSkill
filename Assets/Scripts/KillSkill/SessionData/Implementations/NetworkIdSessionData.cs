using KillSkill.Network;
using Unity.Netcode;
using UnityEngine;

namespace KillSkill.SessionData.Implementations
{
    public class NetworkIdSessionData : ISessionData, INetCodeSerializable
    {
        private string displayName;
        private ulong clientId;

        public string DisplayName => displayName;
        public ulong ClientId => clientId;

        public NetworkIdSessionData()
        {
            displayName = $"Player_{Random.Range(1000, 10000)}";
            Debug.Log($"[NISD] DISPLAY NAME IS {displayName}");
        }

        public void SetClientId(ulong id) => clientId = id;

        public void Serialize(FastBufferWriter writer)
        {
            writer.WriteValueSafe(displayName);
            writer.WriteValueSafe(clientId);
        }

        public void Deserialize(FastBufferReader reader)
        {
            reader.ReadValueSafe(out displayName);
            reader.ReadValueSafe(out clientId);
        }
    }
}