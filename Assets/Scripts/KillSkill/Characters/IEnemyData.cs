using CleverCrow.Fluid.BTs.Trees;
using KillSkill.Skills;
using Skills;

namespace KillSkill.Characters
{
    public interface IEnemyData : ICharacterData
    {
        //todo: SEPARATE INTO ARENA CATALOG DATA!
        public string DisplayName { get; }
        public int CatalogOrder { get; }
        
        //todo: Should definitely be a Stats kinda thing, not here
        public float Health { get; }
        
        //todo: IF we want each enemy possibly have different rewards, put this outside of IEnemyData
        public IResourceReward[] Rewards { get; }
        public Skill[] Skills { get; }
        public BehaviorTreeBuilder OnBuildBehaviourTree(Character character, BehaviorTreeBuilder builder);
    }
}