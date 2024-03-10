using System;
using CleverCrow.Fluid.BTs.Trees;
using KillSkill.Characters;
using KillSkill.Skills;
using Skills;
using Unity.VisualScripting;

namespace KillSkill.Utility.BehaviourTree
{
    public static class BehaviorTreeBuilderExtensions
    {
        public static BehaviorTreeBuilder WaitUntil(this BehaviorTreeBuilder builder, Func<bool> until, string name = "Wait Until")
            => builder.AddNode(new WaitUntil(name, until));
        
        public static BehaviorTreeBuilder ExecuteSkill<T>(this BehaviorTreeBuilder builder, Character executor, bool failOnCantCast = false) where T : Skill
            => builder.AddNode(new ExecuteSkill<T>(executor, failOnCantCast));

        public static BehaviorTreeBuilder WaitForCooldown<T>(this BehaviorTreeBuilder builder, Character executor)
            => builder.AddNode(new WaitForCooldown<T>(executor));
    }
}