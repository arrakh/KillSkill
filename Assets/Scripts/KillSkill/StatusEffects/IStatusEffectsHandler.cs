using System.Collections.Generic;
using KillSkill.StatusEffects;

namespace StatusEffects
{
    public delegate void StatusEffectEvent(IStatusEffect effect);

    public interface IStatusEffectsHandler
    {
        public event StatusEffectEvent OnAdded;
        public event StatusEffectEvent OnRemoved;
        public event StatusEffectEvent OnUpdated;
        
        public void Add(IStatusEffect statusEffect);
        public void Remove<T>() where T : IStatusEffect;
        public void RemoveAny<T>();
        public bool Has<T>();
        public IEnumerable<IStatusEffect> GetAll();
    }
}