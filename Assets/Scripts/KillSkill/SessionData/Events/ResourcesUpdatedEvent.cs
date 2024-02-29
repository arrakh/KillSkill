using SessionData.Implementations;

namespace KillSkill.SessionData.Events
{
    public struct ResourcesUpdatedEvent
    {
        public ResourcesSessionData resourcesSession;

        public ResourcesUpdatedEvent(ResourcesSessionData resourcesSession)
        {
            this.resourcesSession = resourcesSession;
        }
    }
}