using System;
using System.Threading.Tasks;
using Arr.EventsSystem;
using Arr.SDS;
using UnityEngine;
using EventHandler = Arr.EventsSystem.EventHandler;

namespace Arr.ViewModuleSystem
{
    public class View : MonoBehaviour, IScriptableKey
    {
        public string Id => GetType().Name.ToLower();

        private static EventHandler _eventHandler = GlobalEvents.Instance;

        public static void SetEventHandler(EventHandler eventHandler) => _eventHandler = eventHandler;

        public static void Open<T>() where T : View => _eventHandler.Fire(new EventOpenView<T>());
        public static void Close<T>() where T : View => _eventHandler.Fire(new EventCloseView<T>());
        
        public bool IsOpen { get; private set; }

        public virtual bool ActiveOnSpawn => true;

        public void Open()
        {
            if (IsOpen)
            {
                Debug.LogWarning($"You are trying to open a view that is already open!");
                return;
            }
            
            IsOpen = true;
            gameObject.SetActive(true);
            OnOpen();
            _eventHandler.Fire(new EventOnViewOpened(){ view = this });
        }

        public void Close()
        {
            IsOpen = false;
            gameObject.SetActive(false);
            OnClose();
            _eventHandler.Fire(new EventOnViewClosed(){ view = this });
        }

        public async Task Load()
        {
            await OnLoad();
        }

        public async Task Unload()
        {
            await OnUnload();
            _eventHandler.Fire(new EventOnViewClosed(){ view = this });
        }

        protected virtual void OnOpen(){}
        protected virtual void OnClose(){}
        protected virtual async Task OnLoad(){}
        protected virtual async Task OnUnload(){}
    }
}