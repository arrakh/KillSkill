using System;
using Arr.EventsSystem;
using KillSkill.Modules.Battle.Events;
using UnityEngine;

namespace KillSkill.Characters
{
    public interface ICharacterRegistry
    {

        public bool TryGet(uint characterId, out ICharacter character);

        public bool TryRegister(uint characterId, ICharacter character);

        public static ICharacterRegistry GetHandle() => GlobalEvents.Query<QueryCharacterRegistry>().registry;
    }
}