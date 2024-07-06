using System.Collections.Generic;
using KillSkill.Characters;
using UnityEngine;

namespace KillSkill.Minions
{
    public interface ICharacterMinionHandler
    {
        public void Initialize(ICharacterFactory factory, ICharacter owner);
        public ICharacter Add(Vector3 position, ICharacterData data, bool parentToOwner = false);
        public ICollection<ICharacter> GetAll();
        public ICharacter GetRandom();
    }
}