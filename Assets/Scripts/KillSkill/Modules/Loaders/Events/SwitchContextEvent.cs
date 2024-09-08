namespace KillSkill.Modules.Loaders.Events
{
    public struct SwitchContextEvent
    {

        public ContextType contextType;

        public SwitchContextEvent(ContextType contextType)
        {
            this.contextType = contextType;
        }
    }
}