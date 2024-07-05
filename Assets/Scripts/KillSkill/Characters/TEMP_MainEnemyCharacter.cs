using System;
using KillSkill.Modules;
using KillSkill.SessionData;
using KillSkill.SessionData.Implementations;
using UnityEngine;
using VisualEffects;

namespace KillSkill.Characters
{
    public class TEMP_MainEnemyCharacter : EnemyCharacter
    {
        [SerializeField] private CharacterFactoryModule tempFactory; //todo: MUST BE INITIALIZED FROM OUTSIDE
        [SerializeField] private EffectController effectController; //todo: MUST BE INITIALIZED FROM OUTSIDE

        private void Start()
        {
            var battleSession = Session.GetData<BattleSessionData>();

            var data = battleSession.GetEnemy();
            
            Initialize(data, data.Skills, tempFactory, effectController);
        }
    }
}