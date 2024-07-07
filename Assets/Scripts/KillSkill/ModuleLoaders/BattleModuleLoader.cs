using Arr.ModulesSystem;
using DefaultNamespace.SessionData;
using KillSkill.Modules;
using KillSkill.Modules.Battle;
using KillSkill.UI;
using KillSkill.UI.Battle.Modules;
using KillSkill.VisualEffects;

namespace KillSkill.ModuleLoaders
{
    public class BattleModuleLoader : ModulesLoader
    {
        protected override BaseModule[] Modules => new BaseModule[]
        {
            new SessionDataModule(),
            new VisualEffectModule(),
            new CameraControlModule(),
            new PostProcessModule(),
            new UnityEventSystemModule(),
            new TooltipViewModule(),

            new BattleControllerModule(),
            new BattleFactoryModule(),
            
            new CountdownViewModule(),
            new ResultViewModule(),
            new PauseViewModule(),
            new TimerViewModule(),
            new GameSkillIndicatorsViewModule()
        };
    }
}