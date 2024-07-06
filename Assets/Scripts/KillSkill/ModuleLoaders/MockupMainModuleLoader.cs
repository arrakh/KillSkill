using Arr;
using Arr.EventsSystem;
using Arr.ModulesSystem;
using DefaultNamespace.SessionData;
using UnityEngine;

namespace KillSkill.ModuleLoaders
{
    public static class MockupMainModuleLoader
    {
        private static ModulesHandler _handler;

        private static BaseModule[] _modules = {
            new SessionDataModule(),
        };

        [RuntimeInitializeOnLoadMethod]
        public static void Start()
        {
            _handler = new ModulesHandler(_modules, GlobalEvents.Instance);
            _handler.Start().CatchExceptions();
        }
    }
}