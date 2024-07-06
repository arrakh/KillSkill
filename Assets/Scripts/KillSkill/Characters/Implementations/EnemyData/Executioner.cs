using System.Collections.Generic;
using System.Net;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using FlipBooks;
using KillSkill.Characters.Implementations.ResourceRewards;
using KillSkill.Constants;
using KillSkill.Database;
using KillSkill.Skills;
using KillSkill.Skills.Implementations.Enemy;
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
        public string Id => "executioner";

        public string DisplayName => "Executioner";
        public int CatalogOrder => 1;
        public float Health => 800f;

        public IResourceReward[] Rewards => new IResourceReward[]
        {
            new CustomMessageReward(GameResources.COINS, 999,"You beat the demo, congrats :)"),
            new CustomMessageReward(GameResources.COINS, 999,"Look forward to the next iteration of the game!"),
            
            new TimeRangeWinReward(GameResources.MEDALS, 69, 0, 10, "Wow, you beat this guy within 10 seconds? That's crazy"),
        };

        public Skill[] Skills => new Skill[]
        {
            new SlamSkill(),
            new SmashSkill(),
            new PowerChargeSkill(),
            new SpinAttackSkill(),
            new CarefulParrySkill(),
            new SummonFlyingEyeSkill()
        };

        public BehaviorTreeBuilder OnBuildBehaviourTree(ICharacter character, BehaviorTreeBuilder builder)
        {
            //@formatter:off
            return builder
                    .Selector()
                        .Sequence("Summon Flying Eye")
                            .ExecuteSkill<SummonFlyingEyeSkill>(character)
                            .End()
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