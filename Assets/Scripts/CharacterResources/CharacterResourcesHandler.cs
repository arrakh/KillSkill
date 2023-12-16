using System;
using System.Collections.Generic;
using Actors;
using Arr;
using UnityEngine;

namespace CharacterResources
{
    public class CharacterResourcesHandler : IDisposable
    {
        private Character character;

        public delegate void ResourceAssignedDelegate<T>(T resource) where T : ICharacterResource;
        
        private readonly Dictionary<Type, Action> onUnassignedObservers = new();
        private readonly Dictionary<Type, Delegate> onAssignedObservers = new();

        private PersistentEventTemplate<Type, ICharacterResource> onAnyResourceAssigned = new();
        private PersistentEventTemplate<Type> onAnyResourceUnassigned = new();

        private Dictionary<Type, ICharacterResource> resources = new();

        private HashSet<Type> recentlyUnassigned = new();

        public void Assign<T>(T newResource, bool overrideExisting = false) where T : ICharacterResource
        {
            var type = typeof(T);
            if (!overrideExisting && resources.ContainsKey(type))
                throw new Exception($"Resource of type {type} already exist!");

            recentlyUnassigned.Remove(type);

            resources[type] = newResource;
            onAnyResourceAssigned.Invoke(type, newResource);
            if (!onAssignedObservers.TryGetValue(type, out var delegateObj)) return;
            var action = delegateObj as ResourceAssignedDelegate<T>;
            action?.Invoke(newResource);
        }

        public void Unassign<T>() where T : ICharacterResource
        {
            var type = typeof(T);
            bool success = resources.Remove(type);
            if (!success) return;
            recentlyUnassigned.Add(type);
            onAnyResourceUnassigned.Invoke(type);
            if (onUnassignedObservers.TryGetValue(type, out var action)) action.Invoke();
        }

        public bool IsAssigned<T>() where T : ICharacterResource
            => resources.ContainsKey(typeof(T));

        public T Get<T>() where T : ICharacterResource
        {
            if (resources[typeof(T)] is T t) return t;
            throw new Exception($"COULD NOT GET INSTANCE OF TYPE {typeof(T)}");
        }
        
        public bool TryGet<T>(out T instance) where T : ICharacterResource
        {
            instance = default;
            if (!resources.TryGetValue(typeof(T), out var resource)) return false;
            if (resource is not T t) return false;
            instance = t;
            return true;
        }

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
            => onAnyResourceAssigned.Subscribe(onAnyUnassigned);
        
        private bool disposed;
        public void Dispose()
        {
            if (disposed) return;
            onUnassignedObservers.Clear();
            onAssignedObservers.Clear();
            onAnyResourceAssigned = new();
            onAnyResourceUnassigned = new();
            disposed = true;
        }
    }
}