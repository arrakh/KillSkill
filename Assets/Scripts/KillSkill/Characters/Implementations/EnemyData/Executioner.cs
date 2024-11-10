using System;
using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Trees;
using KillSkill.Characters.Implementations.ResourceRewards;
using KillSkill.Constants;
using KillSkill.Skills.Implementations.Enemy;
using KillSkill.Skills.Implementations.Enemy.Executioner;
using KillSkill.Skills.Implementations.Warrior;
using KillSkill.StatusEffects.Implementations;
using KillSkill.Utility.BehaviourTree;

namespace KillSkill.Characters.Implementations.EnemyData
{
    public class Executioner : INpcDefinition, ICataloguedEnemy
    {
        public const string ID = "executioner";
        
        public string Id => ID;

        public string DisplayName => "Executioner";
        public int CatalogOrder => 1;
        public IEnumerable<string> RequiredMilestones => new [] { Milestones.HasDefeated(FlyingEye.ID)};
        public float Health => 800f;

        public IResourceReward[] Rewards => new IResourceReward[]
        {
            new CustomMessageReward(GameResources.COINS, 999,"You beat the demo, congrats :)"),
            new CustomMessageReward(GameResources.COINS, 999,"Look forward to the next iteration of the game!"),
            
            new TimeRangeWinReward(GameResources.MEDALS, 69, 0, 10, "Wow, you beat this guy within 10 seconds? That's crazy"),
        };

        public Type[] SkillTypes => new Type[]
        {
            typeof(HeavySlamSkill),
            typeof(HeavySmashSkill),
            typeof(PowerChargeSkill),
            typeof(SpinAttackSkill),
            typeof(CarefulParrySkill),
            typeof(SummonFlyingEyeSkill)
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
                            .ExecuteSkill<HeavySlamSkill>(character)
                            .WaitForCooldown<HeavySlamSkill>(character)
                            .ExecuteSkill<HeavySmashSkill>(character)
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