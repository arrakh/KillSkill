namespace KillSkill.Modules.Loaders.Events
{
    public struct SwitchContextEvent
    {
        public enum Type
        {
            None,
            Lobby,
            Battle
        }

        public Type contextType;

        public SwitchContextEvent(Type contextType)
        {
            this.contextType = contextType;
        }
    }
}