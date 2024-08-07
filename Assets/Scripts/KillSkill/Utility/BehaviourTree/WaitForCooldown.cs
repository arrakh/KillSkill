﻿using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using KillSkill.Characters;

namespace KillSkill.Utility.BehaviourTree
{
    public class WaitForCooldown<T> : ActionBase
    {
        private ICharacter executor;
        private int skillIndex = int.MaxValue;

        public WaitForCooldown(ICharacter executor)
        {
            Name = $"Waiting for {typeof(T).Name}";

            this.executor = executor;
            
            if (!executor.Skills.TryGetIndex<T>(out skillIndex))
                skillIndex = int.MaxValue;
        }

        protected override TaskStatus OnUpdate()
        {
            if (skillIndex == int.MaxValue) return TaskStatus.Failure;
            var canCast = executor.Skills.CanCast(skillIndex);
            return canCast ? TaskStatus.Success : TaskStatus.Continue;
        }
    }
}