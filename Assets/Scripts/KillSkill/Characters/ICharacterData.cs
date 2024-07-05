using System.Collections.Generic;
using FlipBooks;

namespace KillSkill.Characters
{
    public interface ICharacterData
    {
        //todo: Should definitely be a Stats kinda thing, not here
        public float Health { get; }
        
        public FlipBook DefaultFlipBook { get; }
        public IEnumerable<FlipBook> FlipBooks { get; }
    }
}