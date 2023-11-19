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
            Initialize();

            skills = new Skill[]
            {
                new AttackSkill(0.3f, 2f, 10),
                new BlockStunSkill(5f, 2f, 4f),
                new HealSkill(12f, 35),
                new SacrificeSkill(20f, 50f, 0.5f, 10f)
            };
        }

        protected override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) ExecuteSkill(0, monsterTarget);
            if (Input.GetKeyDown(KeyCode.Alpha2)) ExecuteSkill(1, monsterTarget);
            if (Input.GetKeyDown(KeyCode.Alpha3)) ExecuteSkill(2, this);
            if (Input.GetKeyDown(KeyCode.Alpha4)) ExecuteSkill(3, monsterTarget);
            if (Input.GetKeyDown(KeyCode.Space)) (skills[3], skills[1]) = (skills[1], skills[3]);

        }
    }
}