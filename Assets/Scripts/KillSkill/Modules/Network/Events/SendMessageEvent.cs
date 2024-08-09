using System;
using KillSkill.Network;

namespace KillSkill.Modules.Network.Events
{
    public struct SendMessageEvent
    {
        public readonly ulong clientId;
        public readonly INetCodeMessage message;

        public SendMessageEvent(ulong clientId, INetCodeMessage message)
        {
            this.clientId = clientId;
            this.message = message;
        }
    }
}