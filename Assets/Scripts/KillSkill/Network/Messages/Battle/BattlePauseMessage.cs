using Unity.Netcode;

namespace KillSkill.Network.Messages.Battle
{
    public class BattlePauseMessage : INetCodeMessage
    {
        private bool isPaused;

        public BattlePauseMessage()
        {
        }

        public bool IsPaused => isPaused;

        public BattlePauseMessage(bool isPaused)
        {
            this.isPaused = isPaused;
        }
        
        public void Serialize(FastBufferWriter writer)
        {
            writer.WriteValueSafe(isPaused);
        }

        public void Deserialize(FastBufferReader reader)
        {
            reader.ReadValueSafe(out isPaused);
        }
    }
}