using KillSkill.Network;

namespace KillSkill.Modules.Network.Events
{
    public struct BroadcastMessageEvent
    {
        public readonly INetCodeMessage message;
        
        public BroadcastMessageEvent(INetCodeMessage message)
        {
            this.message = message;
        }
    }
}