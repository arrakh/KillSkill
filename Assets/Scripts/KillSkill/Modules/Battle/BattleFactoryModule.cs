using Arr.ModulesSystem;
using Arr.ScriptableDatabases;
using KillSkill.Battle;
using KillSkill.Characters;
using KillSkill.SessionData.Implementations;
using KillSkill.VisualEffects;
using UnityEngine;

namespace KillSkill.Modules.Battle
{
    public class BattleFactoryModule : BaseModule, ICharacterFactory
    {
        [InjectModule] private VisualEffectModule effectController;

        public NpcCharacter CreateNpc(ICharacterData data)
        {
            var prefab = PrefabRegistry.Get("npc");

            var character = Object.Instantiate(prefab).GetComponent<NpcCharacter>();
            character.Initialize(data, this, effectController);

            return character;
        }

        public PlayerCharacter CreatePlayer(SkillsSessionData skillsSession)
        {
            var prefab = PrefabRegistry.Get("player");

            var character = Object.Instantiate(prefab).GetComponent<PlayerCharacter>();
            character.Initialize(skillsSession, this, effectController);

            return character;
        }

        public BattleLevel CreateLevel(string id)
        {
            var prefab = PrefabRegistry.Get($"level-{id}");
            var level = Object.Instantiate(prefab).GetComponent<BattleLevel>();
            return level;
        }
    }
}