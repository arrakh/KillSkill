namespace KillSkill.Modules.Network.Events
{
    public struct ClientDisconnectedEvent
    {
        public readonly ulong clientId;
        public readonly bool isLocal;

        public ClientDisconnectedEvent(ulong clientId, bool isLocal)
        {
            this.clientId = clientId;
            this.isLocal = isLocal;
        }
    }
}