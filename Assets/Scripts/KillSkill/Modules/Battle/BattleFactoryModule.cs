using System;
using Arr.ModulesSystem;
using Arr.ScriptableDatabases;
using Arr.Utils;
using KillSkill.Battle;
using KillSkill.Characters;
using KillSkill.Network;
using KillSkill.SessionData.Implementations;
using Unity.Netcode;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KillSkill.Modules.Battle
{
    public class BattleFactoryModule : BaseModule, ICharacterFactory
    {

        public NpcCharacter CreateNpc(INpcDefinition definition)
        {
            Debug.Log($"[BFM] WILL BE SPAWNING {definition.Id}".LogColor("cyan"));
            var prefab = PrefabRegistry.Get("npc");
            var obj = Object.Instantiate(prefab);
            var netObj = obj.GetComponent<NetworkObject>();
            netObj.Spawn();
            
            var character = obj.GetComponent<NpcCharacter>();
            character.ServerInitialize(definition, this);
            return character;
        }

        public NpcCharacter CreateNpc<T>() where T : INpcDefinition
        {
            //todo: better implementation of this. shouldn't create an instance of a definition everytime...
            var definition = Activator.CreateInstance<T>();
            return CreateNpc(definition);
        }

        public PlayerCharacter CreatePlayer(SkillsSessionData skillsSession, ulong clientId)
        {
            var prefab = PrefabRegistry.Get("player");
            var obj = Object.Instantiate(prefab);

            var netObj = obj.GetComponent<NetworkObject>();
            netObj.SpawnWithOwnership(clientId);

            var character = obj.GetComponent<PlayerCharacter>();
            character.ServerInitialize(skillsSession, this);

            return character;
        }

        public BattleLevel CreateLevel(string id)
        {
            var prefab = PrefabRegistry.Get($"level-{id}");
            var obj = Object.Instantiate(prefab);

            var netObj = obj.GetComponent<NetworkObject>();
            netObj.Spawn();
            var level = obj.GetComponent<BattleLevel>();
            return level;
        }
    }
}