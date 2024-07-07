using System.Threading.Tasks;
using Arr.ModulesSystem;
using Arr.ScriptableDatabases;
using UnityEngine;
using UnityEngine.Rendering;

namespace KillSkill.Modules
{
    public class PostProcessModule : BaseModule
    {
        private Volume globalVolume;
        
        protected override async Task OnInitialize()
        {
            var prefab = PrefabRegistry.Get("global-volume");
            globalVolume = Object.Instantiate(prefab).GetComponent<Volume>();
        }
    }
}