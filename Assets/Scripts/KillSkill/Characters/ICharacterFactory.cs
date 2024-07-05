using UnityEngine;

namespace KillSkill.Characters
{
    public interface ICharacterFactory
    {
        public Character Create(ICharacterData data);
    }
}