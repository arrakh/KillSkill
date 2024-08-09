namespace KillSkill.Modules.Network.Events
{
    public struct StartJoinEvent
    {
        public readonly string ip;

        public StartJoinEvent(string ip)
        {
            this.ip = ip;
        }
    }
}