using UnityEngine;

namespace KillSkill
{
    public class Timer
    {
        private float originalDuration;
        private float duration;
        private float currentTimer;

        public Timer(float duration, bool automaticallyStart = true)
        {
            originalDuration = this.duration = duration;
            if (automaticallyStart) currentTimer = duration;
        }

        public void Update(float delta)
        {
            currentTimer -= delta;
        }

        public void SetSpeed(float multiplier)
        {
            var progress = NormalizedTime; 
            duration = originalDuration * multiplier;
            currentTimer = duration * progress;
        }

        public void Set(float time)
        {
            duration = time;
            currentTimer = time;
        }

        public void Reset() => currentTimer = duration;

        public float Duration => duration;
        public float NormalizedTime => Mathf.Clamp01(currentTimer / duration);
        public float RemainingTime => Mathf.Clamp(currentTimer, 0, duration);
        public bool IsActive => currentTimer > 0f;
    }
}