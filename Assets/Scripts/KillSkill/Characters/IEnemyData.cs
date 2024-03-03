using CleverCrow.Fluid.BTs.Trees;
using KillSkill.Skills;
using Skills;

namespace KillSkill.Characters
{
    public interface IEnemyData : ICharacterData
    {
        public string DisplayName { get; }
        
        //todo: IF we want each enemy possibly have different rewards, put this outside of IEnemyData
        public IResourceReward[] Rewards { get; }
        public Skill[] Skills { get; }
        public BehaviorTreeBuilder OnBuildBehaviourTree(Character character, BehaviorTreeBuilder builder);
    }
}