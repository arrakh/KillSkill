using System;
using CleverCrow.Fluid.BTs.Trees;
using KillSkill.Skills.Implementations.Warrior;
using KillSkill.Utility.BehaviourTree;

namespace KillSkill.Characters.Implementations.EnemyData
{
    public class FlyingEye : INpcDefinition
    {
        public const string ID = "flying-eye";

        public string Id => ID;
        public float Health => 20;
        public string DisplayName => "Flying Eye";
        public IResourceReward[] Rewards { get; }


        public Type[] SkillTypes => new Type[]
        {
            typeof(SlashSkill)
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