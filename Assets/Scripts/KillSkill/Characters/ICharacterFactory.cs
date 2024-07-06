using UnityEngine;

namespace KillSkill.Characters
{
    public interface ICharacterFactory
    {
        public NpcCharacter CreateNpc(ICharacterData data);
    }
}