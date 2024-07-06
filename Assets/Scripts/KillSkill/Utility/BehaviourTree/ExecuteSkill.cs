using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using KillSkill.Characters;
using KillSkill.Skills;
using Skills;

namespace KillSkill.Utility.BehaviourTree
{
    public class ExecuteSkill<T> : ActionBase where T : Skill
    {
        private ICharacter executor;
        private int skillIndex = int.MaxValue;
        private bool failOnCantCast;

        public ExecuteSkill(ICharacter executor, bool failOnCantCast = false)
        {
            Name = $"Execute {typeof(T).Name}";
            this.executor = executor;
            this.failOnCantCast = failOnCantCast;

            if (!executor.Skills.TryGetIndex<T>(out skillIndex))
                skillIndex = int.MaxValue;
        }

        protected override TaskStatus OnUpdate()
        {
            if (skillIndex == int.MaxValue) return TaskStatus.Failure;
            if (!executor.Skills.CanCast(skillIndex)) return failOnCantCast ? TaskStatus.Failure : TaskStatus.Continue;
            executor.Skills.Execute<T>(executor.Target);
            return TaskStatus.Success;
        }
    }
}