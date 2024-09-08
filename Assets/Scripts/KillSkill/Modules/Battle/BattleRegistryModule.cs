using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arr.ModulesSystem;
using Arr.ScriptableDatabases;
using Arr.EventsSystem;
using Arr.Utils;
using KillSkill.Battle;
using KillSkill.Characters;
using KillSkill.Modules.Battle.Events;
using KillSkill.Network;
using KillSkill.SessionData.Implementations;
using Unity.Netcode;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KillSkill.Modules.Battle
{
    public class BattleRegistryModule : BaseModule, ICharacterRegistry, ICharacterFactory,
        IQueryProvider<QueryCharacterRegistry>
    {
        private Dictionary<uint, ICharacter> characterRegistry = new();
        private uint lastId = 0;
        private bool isServer;

        protected override Task OnLoad()
        {
            isServer = Net.IsServer();
            return base.OnLoad();
        }

        private uint GetId()
        {
            var id = lastId;
            lastId++;
            return id;
        }

        public NpcCharacter CreateNpc(INpcDefinition definition, bool isEnemy = true)
        {
            if (!isServer) throw new Exception("Trying to call CreateNPC NOT from server which is not allowed!");
            var id = GetId();
            Debug.Log($"[BFM] WILL BE SPAWNING {definition.Id} WITH ID {id}".LogColor("cyan"));
            var prefab = PrefabRegistry.Get("npc");
            var obj = Object.Instantiate(prefab);
            var netObj = obj.GetComponent<NetworkObject>();
            netObj.Spawn();
            
            var character = obj.GetComponent<NpcCharacter>();
            character.ServerInitialize(id, isEnemy, definition, this);
            character.onDeath += OnCharacterDeath;

            characterRegistry[id] = character;
            
            return character;
        }

        public NpcCharacter CreateNpc<T>(bool isEnemy = true) where T : INpcDefinition
        {
            //todo: better implementation of this. shouldn't create an instance of a definition everytime...
            var definition = Activator.CreateInstance<T>();
            return CreateNpc(definition, isEnemy);
        }

        public ICharacter[] GetAll(Func<ICharacter, bool> filter = null)
        {
            return filter == null ? characterRegistry.Values.ToArray() 
                : characterRegistry.Values.Where(filter).ToArray();
        }

        public bool TryGet(uint characterId, out ICharacter character)
            => characterRegistry.TryGetValue(characterId, out character);

        public bool TryRegister(uint characterId, ICharacter character)
        {
            if (characterRegistry.ContainsKey(characterId)) return false;

            characterRegistry[characterId] = character;
            return true;
        }

        public PlayerCharacter CreatePlayer(SkillsSessionData skillsSession, ulong clientId)
        {
            if (!isServer) throw new Exception("Trying to call CreatePlayer NOT from server which is not allowed!");

            var id = GetId();

            var prefab = PrefabRegistry.Get("player");
            var obj = Object.Instantiate(prefab);

            var netObj = obj.GetComponent<NetworkObject>();
            netObj.SpawnWithOwnership(clientId);

            var character = obj.GetComponent<PlayerCharacter>();
            character.ServerInitialize(id, skillsSession, this);
            character.onDeath += OnCharacterDeath;

            characterRegistry[id] = character;

            return character;
        }

        public BattleLevel CreateLevel(string id)
        {
            if (!isServer) throw new Exception("Trying to call CreateLevel NOT from server which is not allowed!");

            var prefab = PrefabRegistry.Get($"level-{id}");
            var obj = Object.Instantiate(prefab);

            var netObj = obj.GetComponent<NetworkObject>();
            netObj.Spawn();
            var level = obj.GetComponent<BattleLevel>();
            return level;
        }
        
        private void OnCharacterDeath(ICharacter character)
        {
            character.onDeath -= OnCharacterDeath;
            characterRegistry.Remove(character.Id);
        }

        public QueryCharacterRegistry OnQuery() => new(this);
    }
}