namespace KillSkill.StatusEffects
{
    public interface ITimedStatusEffect
    {
        public void UpdateDuration(float deltaTime);
        public float NormalizedDuration { get; }
        public float RemainingDuration { get; }
    }
}