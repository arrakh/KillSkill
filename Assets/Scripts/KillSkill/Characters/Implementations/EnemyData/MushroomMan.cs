using System;
using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Trees;
using FlipBooks;
using KillSkill.Characters.Implementations.ResourceRewards;
using KillSkill.Constants;
using KillSkill.Database;
using KillSkill.Skills;
using KillSkill.Skills.Implementations.Enemy.Mushroom;
using KillSkill.Skills.Implementations.Fighter;
using KillSkill.Utility.BehaviourTree;
using Skills;
using Unity.Netcode;

namespace KillSkill.Characters.Implementations.EnemyData
{
    public class MushroomMan : INpcDefinition, ICataloguedEnemy
    {
        public string Id => "mushroom-man";

        public string DisplayName => "Mushroom Man";
        public int CatalogOrder => 0;
        public IEnumerable<string> RequiredMilestones => new List<string>();

        public float Health => 250f;

        public IResourceReward[] Rewards => new IResourceReward[]
        {
            new RandomIntReward(GameResources.COINS, 40, 70, 0, 0),
            new TimeRangeWinReward(GameResources.COINS, amount: 45, 8),
            new TimeRangeWinReward(GameResources.COINS, amount: 20, 8, 15),
            new TimeRangeWinReward(GameResources.COINS, amount: 5, 15, 30),
            new TimeRangeWinReward(GameResources.MEDALS, amount: 1, 8),
            new TimeRangeWinReward(GameResources.MEDALS, amount: 1, 20),
        };

        public Type[] SkillTypes => new Type[]
        {
            typeof(SlashSkill),
            typeof(SporePopSkill),
        };

        public BehaviorTreeBuilder OnBuildBehaviourTree(ICharacter character, BehaviorTreeBuilder builder)
        {
            //@formatter:off
            return builder
                .Sequence()
                    .ExecuteSkill<SlashSkill>(character)
                    .ExecuteSkill<SlashSkill>(character)
                    .ExecuteSkill<SlashSkill>(character)
                    .ExecuteSkill<SporePopSkill>(character)
                    .WaitForCooldown<SporePopSkill>(character)
                .End();
            //@formatter:on
        }
    }
}