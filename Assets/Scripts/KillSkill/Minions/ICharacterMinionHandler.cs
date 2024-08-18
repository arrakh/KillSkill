using System.Collections.Generic;
using KillSkill.Characters;
using UnityEngine;

namespace KillSkill.Minions
{
    public interface ICharacterMinionHandler
    {
        public void Initialize(ICharacterFactory registry, ICharacter owner);
        public ICharacter Add<T>(Vector3 position, bool parentToOwner = false) where T : INpcDefinition;
        public ICollection<ICharacter> GetAll();
        public ICharacter GetRandom();
    }
}