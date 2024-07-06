using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Arr
{
    public static class UnityEvents
    {
        public static event Action onUpdate;
        public static event Action<bool> onApplicationFocus;
        public static event Action<bool> onApplicationPause;
        public static event Action onLateUpdate;
        public static event Action onFixedUpdate;

        [RuntimeInitializeOnLoadMethod]
        public static void Init()
        {
            if (UnityEventListener.instance != null) return;
            GameObject go = new("Unity Event Listener") {hideFlags = HideFlags.HideInHierarchy};
            UnityEventListener.instance = go.AddComponent<UnityEventListener>();
            Object.DontDestroyOnLoad(go);
        }

        [DisallowMultipleComponent]
        class UnityEventListener : MonoBehaviour
        {
            public static UnityEventListener instance;

            void Update() => onUpdate?.Invoke();
            void LateUpdate() => onLateUpdate?.Invoke();
            void OnApplicationFocus(bool hasFocus) => onApplicationFocus?.Invoke(hasFocus);
            void OnApplicationPause(bool isPaused) => onApplicationPause?.Invoke(isPaused);

            private void FixedUpdate() => onFixedUpdate?.Invoke();
        }
    }
}