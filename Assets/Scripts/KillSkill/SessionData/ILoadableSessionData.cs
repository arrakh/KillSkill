namespace KillSkill.SessionData
{
    public interface ILoadableSessionData
    {
        public void OnLoad();
        public void OnUnload();
    }
}