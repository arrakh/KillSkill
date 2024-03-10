using System;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using Unity.VisualScripting;

namespace KillSkill.Utility.BehaviourTree
{
    public class WaitUntil : ActionBase
    {
        private Func<bool> condition;

        public WaitUntil(string name, Func<bool> condition)
        {
            Name = name;
            this.condition = condition;
        }

        protected override TaskStatus OnUpdate()
        {
            return condition() ? TaskStatus.Success : TaskStatus.Continue;
        }
    }
}