﻿using System.Collections.Generic;
using System.Net;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using FlipBooks;
using KillSkill.Characters.Implementations.ResourceRewards;
using KillSkill.Constants;
using KillSkill.Database;
using KillSkill.Skills;
using KillSkill.Skills.Implementations.Enemy.Executioner;
using KillSkill.Skills.Implementations.Enemy.Mushroom;
using KillSkill.Skills.Implementations.Fighter;
using KillSkill.Skills.Implementations.HeavyKnight;
using KillSkill.StatusEffects.Implementations;
using KillSkill.Utility.BehaviourTree;
using StatusEffects;
using UnityEngine;

namespace KillSkill.Characters.Implementations.EnemyData
{
    public class Executioner : IEnemyData
    {
        private const string ID = "executioner";
        public FlipBook DefaultFlipBook => CharacterFlipBooksDatabase.Get(ID).Default;
        public IEnumerable<FlipBook> FlipBooks => CharacterFlipBooksDatabase.Get(ID).FlipBooks;

        public string DisplayName => "Executioner";
        public int CatalogOrder => 1;
        public float Health => 600f;

        public IResourceReward[] Rewards => new IResourceReward[]
        {
            new CustomMessageReward(GameResources.COINS, 99999,"You beat the demo, congrats :)"),
            new CustomMessageReward(GameResources.MEDALS, 99,"Look forward to the next iteration of the game!")
        };

        public Skill[] Skills => new Skill[]
        {
            new SlamSkill(),
            new SmashSkill(),
            new PowerChargeSkill(),
            new SpinAttackSkill(),
            new CarefulParrySkill(),
        };

        public BehaviorTreeBuilder OnBuildBehaviourTree(Character character, BehaviorTreeBuilder builder)
        {
            //@formatter:off
            return builder
                    .Selector()
                        .Sequence("Check and React to Casting")
                            .Condition(() => character.Target.StatusEffects.Has<CastingStatusEffect>() && character.Skills.CanCast<CarefulParrySkill>())
                            .WaitTime(0.2f)
                            .ExecuteSkill<CarefulParrySkill>(character)
                            .WaitTime(0.1f)
                            .WaitUntil(() => character.Target.StatusEffects.Has<StaggeredStatusEffect>() || 
                                             (!character.StatusEffects.Has<ParryingStatusEffect>() && !character.StatusEffects.Has<CastingStatusEffect>()), 
                                            "Wait until target is staggered or parry and casting ends")
                            .End()
            
                        .Sequence("React to Staggered")
                            .Condition(() => character.Target.StatusEffects.Has<StaggeredStatusEffect>())
                            .ExecuteSkill<SlamSkill>(character)
                            .WaitForCooldown<SlamSkill>(character)
                            .ExecuteSkill<SmashSkill>(character)
                            .WaitTime(2f)
                            .ExecuteSkill<PowerChargeSkill>(character)
                            .WaitTime(6f)
                            .End()
            
                        .Sequence("Default Action")
                            .Condition(() => character.Skills.CanCast<SpinAttackSkill>())
                            .ExecuteSkill<SpinAttackSkill>(character)
                            .WaitTime(4f)
                            .End()
                    .End();
            //@formatter:on
        }
    }
}