using KillSkill.Modules.Loaders;
using Unity.Netcode;

namespace KillSkill.Network.Messages
{
    public class SwitchContextMessage : INetCodeMessage
    {
        private ContextType type;

        public SwitchContextMessage(ContextType type)
        {
            this.type = type;
        }

        public SwitchContextMessage()
        {
        }

        public ContextType Type => type;
        
        public void Serialize(FastBufferWriter writer)
        {
            var typeByte = (byte) type;
            writer.WriteByteSafe(typeByte);
        }

        public void Deserialize(FastBufferReader reader)
        {
           reader.ReadByteSafe(out byte typeByte);
           type = (ContextType) typeByte;
        }
    }
}