using System;
using KillSkill.Characters;
using UnityEngine;
using VisualEffects;

namespace KillSkill.Modules
{
    //todo: MAKE ACTUAL MODULE
    public class CharacterFactoryModule : MonoBehaviour, ICharacterFactory
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private EffectController effectController; //todo: MUST BE INITIALIZED FROM OUTSIDE

        public Character Create(ICharacterData data)
        {
            if (data is not IEnemyData enemyData) throw new Exception("For now character factory only supports enemies");

            var character = Instantiate(enemyPrefab).GetComponent<Character>();
            character.Initialize(data, enemyData.Skills, this, effectController);

            return character;
        }
    }
}