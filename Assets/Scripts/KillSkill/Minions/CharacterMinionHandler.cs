using System.Collections.Generic;
using System.Linq;
using KillSkill.Characters;
using UnityEngine;

namespace KillSkill.Minions
{
    public class CharacterMinionHandler : ICharacterMinionHandler
    {
        private ICharacterFactory characterFactory;
        private Character character;
        private Dictionary<int, Character> minions = new();

        public void Initialize(ICharacterFactory factory, Character ownerChar)
        {
            characterFactory = factory;
            character = ownerChar;
            character.onDeath += OnCharacterDeath;
        }

        private void OnCharacterDeath(Character obj)
        {
            character.onDeath -= OnCharacterDeath;
            foreach (var minion in minions.Values)
                minion.Kill();
            minions.Clear();
        }
        
        public Character Add(Vector3 position, ICharacterData data, bool parentToOwner = false)
        {
            var minion = characterFactory.Create(data);
            
            //should be impossible
            if (minions.Remove(minion.Uid, out var existing)) Object.Destroy(existing.gameObject);
            
            if (parentToOwner) minion.transform.SetParent(character.transform);

            minion.transform.position = position;

            minions[minion.Uid] = minion;

            minion.onDeath += OnMinionDeath;

            return minion;
        }

        private void OnMinionDeath(Character minion)
        {
            minion.onDeath -= OnMinionDeath;
            minions.Remove(minion.Uid);
        }

        public ICollection<Character> GetAll() => minions.Values;

        public Character GetRandom() => minions.Values.ToArray()[Random.Range(0, minions.Count)];
    }
}