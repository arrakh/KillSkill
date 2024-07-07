using System.Threading.Tasks;
using Arr.ModulesSystem;
using Arr.ScriptableDatabases;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

namespace KillSkill.Modules
{
    public class UnityEventSystemModule : BaseModule
    {
        private GameObject eventSystem;
        
        protected override async Task OnInitialize()
        {
            var prefab = PrefabRegistry.Get("unity-event-system");
            eventSystem = Object.Instantiate(prefab);
        }
    }
}