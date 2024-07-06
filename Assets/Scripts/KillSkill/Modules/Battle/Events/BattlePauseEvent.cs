namespace KillSkill.Modules.Battle.Events
{
    public struct BattlePauseEvent
    {
        public readonly bool paused;

        public BattlePauseEvent(bool paused)
        {
            this.paused = paused;
        }
    }
}