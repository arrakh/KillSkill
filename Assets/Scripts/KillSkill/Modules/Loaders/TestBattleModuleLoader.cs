using Arr.ModulesLoader;
using Arr.ModulesSystem;
using KillSkill.Modules.Battle;
using KillSkill.Modules.Groups;
using KillSkill.UI;
using KillSkill.UI.Battle.Modules;

namespace KillSkill.Modules.Loaders
{
    public class TestBattleModuleLoader : MonoModulesLoader
    {
        protected override IModule[] Modules => new IModule[]
        {
            new MasterModuleGroup(),
            new BattleModuleGroup()
        };
    }
}