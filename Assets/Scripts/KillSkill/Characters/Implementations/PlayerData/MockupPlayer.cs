using System.Collections.Generic;
using FlipBooks;
using KillSkill.Database;

namespace KillSkill.Characters.Implementations.PlayerData
{
    public class MockupPlayer : ICharacterData
    {
        private const string ID = "mockup-player";
        public float Health => 25000;
        public FlipBook DefaultFlipBook => CharacterFlipBooksDatabase.Get(ID).Default;
        public IEnumerable<FlipBook> FlipBooks => CharacterFlipBooksDatabase.Get(ID).FlipBooks;
    }
}