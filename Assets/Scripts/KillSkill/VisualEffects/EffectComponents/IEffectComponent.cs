using KillSkill.VisualEffects;

namespace VisualEffects.EffectComponents
{
    public interface IEffectComponent
    {
        public void Initialize(UnityEffect effect, IEffectPool pool);
    }
}