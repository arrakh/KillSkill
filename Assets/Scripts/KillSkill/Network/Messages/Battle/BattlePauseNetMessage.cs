using Unity.Netcode;

namespace KillSkill.Network.Messages.Battle
{
    public class BattlePauseNetMessage : INetCodeMessage
    {
        private bool isPaused;

        public BattlePauseNetMessage()
        {
        }

        public bool IsPaused => isPaused;

        public BattlePauseNetMessage(bool isPaused)
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