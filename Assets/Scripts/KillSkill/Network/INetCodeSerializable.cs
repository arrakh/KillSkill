using Unity.Netcode;

namespace KillSkill.Network
{
    public interface INetCodeSerializable
    {
        public void Serialize(FastBufferWriter writer);
        public void Deserialize(FastBufferReader reader);
    }
}