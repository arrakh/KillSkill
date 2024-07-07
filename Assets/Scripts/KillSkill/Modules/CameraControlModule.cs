using System.Threading.Tasks;
using Arr.ModulesSystem;
using Arr.ScriptableDatabases;
using UnityEngine;

namespace KillSkill.Modules
{
    public class CameraControlModule : BaseModule
    {
        public CameraController Controller => controller;
        
        private CameraController controller;

        protected override async Task OnLoad()
        {
            var prefab = PrefabRegistry.Get("camera-controller");
            controller = Object.Instantiate(prefab).GetComponent<CameraController>();
        }
    }
}