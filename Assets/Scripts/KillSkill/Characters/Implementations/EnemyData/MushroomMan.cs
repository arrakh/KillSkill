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

namespace KillSkill.Characters.Implementations.EnemyData
{
    public class MushroomMan : IEnemyData
    {
        private const string ID = "mushroom-man";
        public FlipBook DefaultFlipBook => CharacterFlipBooksDatabase.Get(ID).Default;
        public IEnumerable<FlipBook> FlipBooks => CharacterFlipBooksDatabase.Get(ID).FlipBooks;

        public string DisplayName => "Mushroom Man";
        public int CatalogOrder => 0;
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

        public Skill[] Skills => new Skill[]
        {
            new SlashSkill(),
            new SporePopSkill(),
        };

        public BehaviorTreeBuilder OnBuildBehaviourTree(Character character, BehaviorTreeBuilder builder)
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