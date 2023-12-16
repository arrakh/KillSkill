using System;
using System.Linq;
using Arr.EventsSystem;
using UnityEngine;
using EventHandler = Arr.EventsSystem.EventHandler;

namespace Arr.ModulesSystem
{
    public abstract class ModulesLoader : MonoBehaviour
    {
        protected abstract BaseModule[] Modules { get; }
        
        protected virtual EventHandler EventHandler => GlobalEvents.Instance;

        private ModulesHandler modulesHandler;
        
        private void Start()
        {
            modulesHandler = new ModulesHandler(Modules, EventHandler);
            
            modulesHandler.Start().CatchExceptions();
        }

        private void OnDestroy()
        {
            modulesHandler.Stop().CatchExceptions();
        }
    }
}