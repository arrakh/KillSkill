using UnityEngine;

namespace KillSkill.Battle
{
    public class BattleLevel : MonoBehaviour
    {
        [SerializeField] private Transform playerSpawnPoint, enemySpawnPoint;

        public Transform PlayerSpawnPoint => playerSpawnPoint;
        public Transform EnemySpawnPoint => enemySpawnPoint;
    }
}