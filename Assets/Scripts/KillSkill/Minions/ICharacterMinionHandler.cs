using System.Collections.Generic;
using KillSkill.Characters;
using UnityEngine;

namespace KillSkill.Minions
{
    public interface ICharacterMinionHandler
    {
        public void Initialize(ICharacterFactory factory, Character owner);
        public Character Add(Vector3 position, ICharacterData data, bool parentToOwner = false);
        public ICollection<Character> GetAll();
        public Character GetRandom();
    }
}