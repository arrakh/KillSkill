using System;
using System.Collections;
using System.Collections.Generic;
using Skills;
using UnityEngine;

namespace Actors
{
    public class Monster : Character
    {
        [SerializeField] private Character playerTarget;

        IEnumerator Start()
        { 
            Initialize();
            skills = new Skill[]
            {
                new AttackSkill(),
                new SporePopSkill()
            };
            
            while (isAlive)
            {
                yield return new WaitUntil(() => !battlePause);
                for (int i = 0; i < 3; i++)
                {
                    yield return new WaitUntil(() => GetSkill(0).CanExecute(this) && !battlePause);
                    ExecuteSkill(0, playerTarget);
                }

                yield return new WaitUntil(() => GetSkill(1).CanExecute(this) && !battlePause);
                ExecuteSkill(1, playerTarget);

                yield return new WaitForSeconds(10f);
            }
        }
    }
}