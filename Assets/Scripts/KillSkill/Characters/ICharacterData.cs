using System.Collections.Generic;
using FlipBooks;

namespace KillSkill.Characters
{
    public interface ICharacterData
    {
        public FlipBook DefaultFlipBook { get; }
        public IEnumerable<FlipBook> FlipBooks { get; }
    }
}