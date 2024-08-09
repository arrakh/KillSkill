namespace KillSkill.Modules.Network.Events
{
    public struct IsServerQuery
    {
        public bool isServer;

        public IsServerQuery(bool isServer)
        {
            this.isServer = isServer;
        }
    }
}