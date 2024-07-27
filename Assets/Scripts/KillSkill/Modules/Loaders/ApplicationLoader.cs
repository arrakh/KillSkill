using System;
using System.Threading;
using System.Threading.Tasks;
using Arr.EventsSystem;
using Arr.ModulesSystem;
using Arr.Utils;
using KillSkill.Modules.Groups;
using KillSkill.Modules.Loaders.Events;
using UnityEngine;

namespace KillSkill.Modules.Loaders
{
    public class ApplicationLoader : MonoBehaviour, IExternalModulesProvider, 
        IEventListener<SwitchContextEvent>,
        IEventListener<QuitLobbyEvent>
    {
        private ModulesHandler masterModulesHandler, lobbyModulesHandler, battleModulesHandler;
        private ModulesHandler currentContext, nextContext;

        private readonly CancellationTokenSource stopAppToken = new();
        private TaskCompletionSource<ModulesHandler> changeContext;


        private async void Start()
        {
            var eventHandler = GlobalEvents.Instance;
            eventHandler.RegisterMultiple(this);
            masterModulesHandler = new ModulesHandler(new MasterModuleGroup().Modules, eventHandler);
            lobbyModulesHandler = new ModulesHandler(new LobbyModuleGroup().Modules, eventHandler, this);
            battleModulesHandler = new ModulesHandler(new BattleModuleGroup().Modules, eventHandler, this);

            Debug.Log("Loading Master Modules");
            await masterModulesHandler.Start();
            
            Debug.Log("Loading Lobby Modules");
            currentContext = lobbyModulesHandler;
            await currentContext.Start();

            changeContext = new();

            while (!stopAppToken.IsCancellationRequested)
            {
                var newContext = await changeContext.Task;
                changeContext = new();

                await currentContext.Stop();
                currentContext = newContext;
                await currentContext.Start();
            }

            await currentContext.Stop();
            await masterModulesHandler.Stop();
            eventHandler.UnregisterMultiple(this);

            Application.Quit();
        }

        public void OnEvent(SwitchContextEvent data)
        {
            switch (data.contextType)
            {
                case SwitchContextEvent.Type.Lobby:
                    changeContext.SetResult(lobbyModulesHandler);
                    break;
                case SwitchContextEvent.Type.Battle:
                    changeContext.SetResult(battleModulesHandler);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDestroy()
        {
            GlobalEvents.Instance.UnregisterMultiple(this);
            masterModulesHandler.Stop().CatchExceptions();
            currentContext.Stop().CatchExceptions();
        }

        public void OnEvent(QuitLobbyEvent data)
        {
            stopAppToken.Cancel();
        }

        public bool TryGetModule(Type type, out IModule instance)
            => masterModulesHandler.TryGetModule(type, out instance);
    }
}