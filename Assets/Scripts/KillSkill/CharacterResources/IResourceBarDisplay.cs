using System;
using KillSkill.Characters;
using UI;

namespace CharacterResources
{
    public interface IResourceBarDisplay
    {
        public event Action<ResourceBarDisplay> OnUpdateDisplay; 
        public ResourceBarDisplay DisplayData { get; }
    }
}