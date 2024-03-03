using CleverCrow.Fluid.BTs.Trees;
using KillSkill.Characters;
using KillSkill.Skills;
using Skills;

namespace KillSkill.Utility.BehaviourTree
{
    public static class BehaviorTreeBuilderExtensions
    {
        public static BehaviorTreeBuilder ExecuteSkill<T>(this BehaviorTreeBuilder builder, Character executor, bool failOnCantCast = false) where T : Skill
            => builder.AddNode(new ExecuteSkill<T>(executor, failOnCantCast));

        public static BehaviorTreeBuilder WaitForCooldown<T>(this BehaviorTreeBuilder builder, Character executor)
            => builder.AddNode(new WaitForCooldown<T>(executor));
    }
}