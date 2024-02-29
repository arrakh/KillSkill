using System;
using Actors;
using UnityEngine;

namespace UI
{
    public class TEMP_ResourceDisplayController : MonoBehaviour
    {
        [SerializeField] private Character player, monster;
        [SerializeField] private CharacterResourcesDisplay playerDisplay, monsterDisplay;
        
        private void Awake()
        {
            playerDisplay.Initialize(player);
            monsterDisplay.Initialize(monster);
        }
    }
}