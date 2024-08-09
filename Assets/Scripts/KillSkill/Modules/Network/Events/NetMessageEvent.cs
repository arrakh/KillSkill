using KillSkill.Network;

namespace KillSkill.Modules.Network.Events
{
    public struct NetMessageEvent<T> where T : INetCodeMessage
    {
        public readonly ulong senderId;
        public readonly T message;
        
        public NetMessageEvent(ulong senderId, T message)
        {
            this.senderId = senderId;
            this.message = message;
        }
    }
}