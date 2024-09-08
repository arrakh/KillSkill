using VisualEffects;

namespace KillSkill.VisualEffects
{
    public interface IEffectPool
    {
        public UnityEffect Get();

        public void Return(UnityEffect effect);
    }
}