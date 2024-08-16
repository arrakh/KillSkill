﻿using System;
using System.Collections.Generic;
using System.Linq;
using KillSkill.Characters;
using Unity.Netcode;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace KillSkill.Minions
{
    public class CharacterMinionHandler : NetworkBehaviour, ICharacterMinionHandler
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

        public ICharacter Add<T>(Vector3 position, bool parentToOwner = false) where T : INpcDefinition
        {
            var minion = characterFactory.CreateNpc<T>();
            
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