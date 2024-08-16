using System;
using UnityEngine;

namespace KillSkill.Characters
{
    public interface ICharacterFactory
    {
        public NpcCharacter CreateNpc(INpcDefinition npcType);
        public NpcCharacter CreateNpc<T>() where T : INpcDefinition;
    }
}