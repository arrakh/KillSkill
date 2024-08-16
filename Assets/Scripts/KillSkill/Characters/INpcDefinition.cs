using System;
using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Trees;
using KillSkill.Skills;
using Skills;

namespace KillSkill.Characters
{
    public interface INpcDefinition
    {
        public string DisplayName { get; }
        
        public string Id { get; }
        
        public float Health { get; }
        
        public Type[] SkillTypes { get; }

        //todo: IF we want each enemy possibly have different rewards, put this outside of IEnemyData
        public IResourceReward[] Rewards { get; }
        
        public BehaviorTreeBuilder OnBuildBehaviourTree(ICharacter character, BehaviorTreeBuilder builder);
    }
}