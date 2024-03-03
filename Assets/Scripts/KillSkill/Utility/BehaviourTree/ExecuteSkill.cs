using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using KillSkill.Characters;
using KillSkill.Skills;
using Skills;

namespace KillSkill.Utility.BehaviourTree
{
    public class ExecuteSkill<T> : ActionBase where T : Skill
    {
        private Character executor;
        private Character target;
        private int skillIndex = int.MaxValue;
        private bool failOnCantCast;

        public ExecuteSkill(Character executor, bool failOnCantCast = false)
        {
            Name = $"Execute {typeof(T).Name}";
            this.executor = executor;
            target = executor.Target;
            this.failOnCantCast = failOnCantCast;

            if (!executor.TryGetSkillIndex<T>(out skillIndex))
                skillIndex = int.MaxValue;
        }

        protected override TaskStatus OnUpdate()
        {
            if (skillIndex == int.MaxValue) return TaskStatus.Failure;
            if (!executor.CanCastAbility(skillIndex)) return failOnCantCast ? TaskStatus.Failure : TaskStatus.Continue;
            executor.ExecuteSkill<T>(target);
            return TaskStatus.Success;
        }
    }
}