using System.Collections.Generic;
using System.Linq;
using KillSkill.Characters;
using UnityEngine;

namespace KillSkill.Minions
{
    public class CharacterMinionHandler : ICharacterMinionHandler
    {
        private ICharacterFactory characterFactory;
        private ICharacter character;
        private Dictionary<int, ICharacter> minions = new();

        public void Initialize(ICharacterFactory factory, ICharacter ownerChar)
        {
            characterFactory = factory;
            character = ownerChar;
            character.onDeath += OnCharacterDeath;
        }

        private void OnCharacterDeath(ICharacter obj)
        {
            character.onDeath -= OnCharacterDeath;
            foreach (var minion in minions.Values)
                minion.Kill();
            minions.Clear();
        }
        
        public ICharacter Add(Vector3 position, ICharacterData data, bool parentToOwner = false)
        {
            var minion = characterFactory.CreateNpc(data);
            
            //should be impossible
            if (minions.Remove(minion.Uid, out var existing)) Object.Destroy(existing.GameObject);
            
            if (parentToOwner) minion.GameObject.transform.SetParent(character.GameObject.transform);

            minion.GameObject.transform.position = position;

            minions[minion.Uid] = minion;

            minion.onDeath += OnMinionDeath;

            return minion;
        }

        private void OnMinionDeath(ICharacter minion)
        {
            minion.onDeath -= OnMinionDeath;
            minions.Remove(minion.Uid);
        }

        public ICollection<ICharacter> GetAll() => minions.Values;

        public ICharacter GetRandom() => minions.Values.ToArray()[Random.Range(0, minions.Count)];
    }
}