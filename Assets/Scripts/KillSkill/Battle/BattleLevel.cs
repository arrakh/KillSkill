using Unity.Netcode;
using UnityEngine;

namespace KillSkill.Battle
{
    public class BattleLevel : NetworkBehaviour
    {
        [SerializeField] private Transform[] playerSpawnPoints;
        [SerializeField] private Transform enemySpawnPoint;

        public Transform[] PlayerSpawnPoints => playerSpawnPoints;
        public Transform EnemySpawnPoint => enemySpawnPoint;
    }
}