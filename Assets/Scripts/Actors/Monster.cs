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
        
        private void Awake()
        {
            Initialize();
            skills = new Skill[]
            {
                new AttackSkill(0.3f, 3f, 10f),
                new SporePopSkill(10f, 3f, 120f, 5f)
            };
        }

        IEnumerator Start()
        { 
            while (true)
            {
                for (int i = 0; i < 3; i++)
                {
                    yield return new WaitUntil(() => GetSkill(0).CanExecute(this));
                    ExecuteSkill(0, playerTarget);
                }

                yield return new WaitUntil(() => GetSkill(1).CanExecute(this));
                ExecuteSkill(1, playerTarget);

                yield return new WaitForSeconds(10f);
            }
        }
    }
}