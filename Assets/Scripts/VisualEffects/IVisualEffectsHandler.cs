using UnityEngine;

namespace VisualEffects
{
    public interface IVisualEffectsHandler
    {
        public IEffect Spawn(string key, Vector3 position);
    }
}