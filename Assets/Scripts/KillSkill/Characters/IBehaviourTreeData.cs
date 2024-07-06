using CleverCrow.Fluid.BTs.Trees;

namespace KillSkill.Characters
{
    public interface IBehaviourTreeData
    {
        public BehaviorTreeBuilder OnBuildBehaviourTree(ICharacter character, BehaviorTreeBuilder builder);
    }
}