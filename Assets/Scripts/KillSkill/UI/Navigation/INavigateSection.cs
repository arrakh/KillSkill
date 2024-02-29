namespace KillSkill.UI.Navigation
{
    public interface INavigateSection
    {
        public int Order { get; }
        public string Name { get; }
        public void OnNavigate(bool selected);
    }
}