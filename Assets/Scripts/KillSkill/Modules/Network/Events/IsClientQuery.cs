namespace KillSkill.Modules.Network.Events
{
    public struct IsClientQuery
    {
        public bool isClient;

        public IsClientQuery(bool isClient)
        {
            this.isClient = isClient;
        }
    }
}