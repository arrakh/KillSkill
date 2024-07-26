using Arr.EventsSystem;
using Arr.ModulesLoader;
using Arr.ModulesSystem;
using Arr.Utils;
using KillSkill.Modules.Groups;

namespace KillSkill.Modules.Loaders
{
    public class TestLobbyModuleLoader : MonoModulesLoader
    {
        protected override IModule[] Modules => new IModule[]
        {
            new MasterModuleGroup(),
            new LobbyModuleGroup()
        };
    }
}