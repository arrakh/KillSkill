using Actors;
using UI;

namespace CharacterResources
{
    public interface IResourceBarDisplay
    {
        public ResourceBarDisplaySettings GetDisplaySettings(Character character);
    }
}