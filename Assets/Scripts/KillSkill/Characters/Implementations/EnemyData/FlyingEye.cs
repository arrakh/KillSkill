using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using FlipBooks;
using KillSkill.Database;
using KillSkill.Skills;
using KillSkill.Skills.Implementations.Fighter;
using KillSkill.Utility.BehaviourTree;
using UnityEngine;

namespace KillSkill.Characters.Implementations.EnemyData
{
    public class FlyingEye : IEnemyData
    {
        private const string ID = "flying-eye";
        public float Health => 20;
        public FlipBook DefaultFlipBook => CharacterFlipBooksDatabase.Get(ID).Default;
        public IEnumerable<FlipBook> FlipBooks => CharacterFlipBooksDatabase.Get(ID).FlipBooks;
        public string DisplayName => "Flying Eye";
        public int CatalogOrder => 3;
        public IResourceReward[] Rewards { get; }

        public Skill[] Skills => new Skill[]
        {
            new SlashSkill()
        };
        
        public BehaviorTreeBuilder OnBuildBehaviourTree(ICharacter character, BehaviorTreeBuilder builder)
        {
            //@formatter:off
            return builder
                .Sequence()
                    .ExecuteSkill<SlashSkill>(character)
                    .WaitTime(3f)
                .End();
            //@formatter:on
        }
    }
}