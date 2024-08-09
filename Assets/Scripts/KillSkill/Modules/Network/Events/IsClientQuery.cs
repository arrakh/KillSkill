namespace KillSkill.Modules.Network.Events
{
    public class IsClientQuery
    {
        public bool isClient;

        public IsClientQuery(bool isClient)
        {
            this.isClient = isClient;
        }
    }
}