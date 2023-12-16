using System.Threading.Tasks;
using Arr.ModulesSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Arr.MiscModules
{
    public class UnityEventSystemModule : BaseModule
    {
        private GameObject gameObject;
        
        protected override async Task OnLoad()
        {
            gameObject = Resources.Load<GameObject>("EventSystem");
            Object.Instantiate(gameObject);
        }

        protected override async Task OnUnload()
        {
            
        }
    }
}