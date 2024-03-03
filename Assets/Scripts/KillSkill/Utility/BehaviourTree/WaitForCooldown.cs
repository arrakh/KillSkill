using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using KillSkill.Characters;

namespace KillSkill.Utility.BehaviourTree
{
    public class WaitForCooldown<T> : ActionBase
    {
        private Character executor;
        private int skillIndex = int.MaxValue;

        public WaitForCooldown(Character executor)
        {
            Name = $"Waiting for {typeof(T).Name}";

            this.executor = executor;
            
            if (!executor.TryGetSkillIndex<T>(out skillIndex))
                skillIndex = int.MaxValue;
        }

        protected override TaskStatus OnUpdate()
        {
            if (skillIndex == int.MaxValue) return TaskStatus.Failure;
            var canCast = executor.CanCastAbility(skillIndex);
            return canCast ? TaskStatus.Success : TaskStatus.Continue;
        }
    }
}