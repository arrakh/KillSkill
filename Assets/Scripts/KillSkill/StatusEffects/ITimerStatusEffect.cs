namespace KillSkill.StatusEffects
{
    public interface ITimerStatusEffect
    {
        public void UpdateDuration(float deltaTime);
        public float NormalizedDuration { get; }
        public float RemainingDuration { get; }
    }
}