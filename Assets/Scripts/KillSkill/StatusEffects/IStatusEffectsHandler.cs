using System.Collections.Generic;

namespace StatusEffects
{
    public delegate void StatusEffectEvent(StatusEffect effect);

    public interface IStatusEffectsHandler
    {
        public event StatusEffectEvent OnAdded;
        public event StatusEffectEvent OnRemoved;
        public event StatusEffectEvent OnUpdated;
        
        public void Add(StatusEffect statusEffect);
        public void Remove<T>() where T : StatusEffect;
        public void RemoveAny<T>();
        public bool Has<T>();
        public IEnumerable<StatusEffect> GetAll();
    }
}