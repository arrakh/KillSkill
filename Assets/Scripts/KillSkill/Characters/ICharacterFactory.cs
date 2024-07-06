using UnityEngine;

namespace KillSkill.Characters
{
    public interface ICharacterFactory
    {
        public ICharacter Create(ICharacterData data);
    }
}