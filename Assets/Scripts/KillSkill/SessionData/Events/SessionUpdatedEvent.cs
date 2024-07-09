namespace KillSkill.SessionData.Events
{
    public struct SessionUpdatedEvent<T> where T : ISessionData
    {
        public T session;

        public SessionUpdatedEvent(T session)
        {
            this.session = session;
        }
    }
}