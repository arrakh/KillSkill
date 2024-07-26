using Arr.ModulesSystem;
using KillSkill.SessionData;
using KillSkill.UI;
using KillSkill.VisualEffects;

namespace KillSkill.Modules.Groups
{
    public class MasterModuleGroup : ModuleGroup
    {
        public override IModule[] Modules => new IModule[]
        {
            new SessionDataModule(),
            new VisualEffectModule(),
            new CameraControlModule(),
            new UnityEventSystemModule(),
            new TooltipViewModule(),
            new SkillSessionModule()
        };
    }
}