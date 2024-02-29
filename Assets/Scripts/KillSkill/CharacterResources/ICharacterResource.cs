using System;
using Actors;

namespace CharacterResources
{
    public interface ICharacterResource
    {
        public event Action<double, double> OnUpdated;
    }
}