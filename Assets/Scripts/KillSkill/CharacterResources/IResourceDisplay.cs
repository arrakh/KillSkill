using System;

namespace KillSkill.CharacterResources
{
    public interface IResourceDisplay<out T>
    {
        public event Action<T> OnUpdateDisplay; 
        public T DisplayData { get; }
    }
}