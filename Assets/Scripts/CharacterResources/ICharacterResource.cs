using Actors;

namespace CharacterResources
{
    public interface ICharacterResource
    {
        public bool HasAny();
        public double Get();
        public bool TrySet(double value, Character instigator);
        public bool TryAdd(double delta, Character instigator);
    }
}