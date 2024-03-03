using System;
using System.Collections.Generic;
using Arr;
using CharacterResources;

namespace KillSkill.CharacterResources
{
    public partial class CharacterResourcesHandler
    {
        public delegate void ResourceAssignedDelegate<T>(T resource) where T : ICharacterResource;
        
        private readonly Dictionary<Type, Action> onUnassignedObservers = new();
        private readonly Dictionary<Type, Delegate> onAssignedObservers = new();

        private PersistentEventTemplate<Type, ICharacterResource> onAnyResourceAssigned = new();
        private PersistentEventTemplate<Type> onAnyResourceUnassigned = new();

        public void ObserveUnassigned<T>(Action onUnassigned, bool persistent = true) where T : ICharacterResource
        {
            var type = typeof(T);
            if (persistent && recentlyUnassigned.Contains(type)) onUnassigned.Invoke();
            
            onUnassignedObservers[type] += onUnassigned;
        }

        public void ObserveAssigned<T>(ResourceAssignedDelegate<T> onAssigned, bool persistent = true) where T : ICharacterResource
        {
            var type = typeof(T);
            if (persistent && resources.TryGetValue(type, out var resource)) 
                onAssigned.Invoke((T)resource);

            if (!onAssignedObservers.ContainsKey(type)) onAssignedObservers[type] = onAssigned;
            else onAssignedObservers[type] = Delegate.Combine(onAssignedObservers[type], onAssigned);
        }

        public void ObserveAnyAssigned(EventTemplate<Type, ICharacterResource>.EventHandler onAnyAssigned)
            => onAnyResourceAssigned.Subscribe(onAnyAssigned);

        public void ObserveAnyUnassigned(EventTemplate<Type>.EventHandler onAnyUnassigned)
            => onAnyResourceUnassigned.Subscribe(onAnyUnassigned);
        
        public void UnobserveUnassigned<T>(Action onUnassigned) where T : ICharacterResource
        {
            var type = typeof(T);
            if (onUnassignedObservers.TryGetValue(type, out var existingAction))
            {
                var newAction = (Action)Delegate.Remove(existingAction, onUnassigned);
                if (newAction == null)
                    onUnassignedObservers.Remove(type);
                else
                    onUnassignedObservers[type] = newAction;
            }
        }

        public void UnobserveAssigned<T>(ResourceAssignedDelegate<T> onAssigned) where T : ICharacterResource
        {
            var type = typeof(T);
            if (onAssignedObservers.TryGetValue(type, out var existingDelegate))
            {
                onAssignedObservers[type] = Delegate.Remove(existingDelegate, onAssigned);
                if (onAssignedObservers[type] == null)
                {
                    onAssignedObservers.Remove(type);
                }
            }
        }

        public void UnobserveAnyAssigned(EventTemplate<Type, ICharacterResource>.EventHandler onAnyAssigned)
        {
            onAnyResourceAssigned.Unsubscribe(onAnyAssigned);
        }

        public void UnobserveAnyUnassigned(EventTemplate<Type>.EventHandler onAnyUnassigned)
        {
            onAnyResourceUnassigned.Unsubscribe(onAnyUnassigned);
        }
    }
}