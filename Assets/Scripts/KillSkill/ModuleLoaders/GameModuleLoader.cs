using Arr.ModulesSystem;
using DefaultNamespace.SessionData;

namespace KillSkill.ModuleLoaders
{
    public class GameModuleLoader : ModulesLoader
    {
        protected override BaseModule[] Modules => new[]
        {
            new SessionDataModule()
        };
    }
}