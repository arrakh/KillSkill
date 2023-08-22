using System;
using Skills;
using UnityEngine;

namespace Actors
{
    public class Player : Character
    {
        [SerializeField] private Character monsterTarget;
        
        private void Awake()
        {
            skills = new Skill[]
            {
                new AttackSkill(2f, 10),
                new BlockSkill(5f, 2f),
                new HealSkill(4f, 5)
            };
        }

        protected override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) ExecuteSkill(0, monsterTarget);
            if (Input.GetKeyDown(KeyCode.Alpha2)) ExecuteSkill(1, monsterTarget);
            if (Input.GetKeyDown(KeyCode.Alpha3)) ExecuteSkill(2, this);
        }
    }
}