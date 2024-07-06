using Arr.ModulesSystem;
using DefaultNamespace.SessionData;
using KillSkill.Modules.Battle;
using KillSkill.UI.Game.Modules;
using KillSkill.VisualEffects;

namespace KillSkill.ModuleLoaders
{
    public class BattleModuleLoader : ModulesLoader
    {
        protected override BaseModule[] Modules => new BaseModule[]
        {
            new SessionDataModule(),
            new VisualEffectModule(),
            
            new BattleControllerModule(),
            new BattleFactoryModule(),
            
            new CountdownViewModule(),
            new ResultViewModule(),
            new TimerViewModule(),
        };
    }
}