﻿using System;
using System.Collections.Generic;
using System.Linq;
using Arr;
using Arr.EventsSystem;
using CharacterResources;
using Unity.Netcode;

namespace KillSkill.CharacterResources
{
    public partial class CharacterResourcesHandler : NetworkBehaviour, IDisposable, ICharacterResourcesHandler
    {
        private Dictionary<Type, ICharacterResource> resources = new();

        private HashSet<Type> recentlyUnassigned = new();

        public IReadOnlyDictionary<Type, ICharacterResource> Current => resources;

        public void Assign<T>(T newResource, bool overrideExisting = false) where T : ICharacterResource
        {
            var type = typeof(T);
            if (!overrideExisting && resources.ContainsKey(type))
                throw new Exception($"Resource of type {type} already exist!");

            recentlyUnassigned.Remove(type);

            resources[type] = newResource;
            
            GlobalEvents.RegisterMultipleUnsafe(newResource);
            
            onAnyResourceAssigned.Invoke(type, newResource);
            if (!onAssignedObservers.TryGetValue(type, out var delegateObj)) return;
            var action = delegateObj as ResourceAssignedDelegate<T>;
            action?.Invoke(newResource);
        }

        public void Unassign<T>() where T : ICharacterResource
        {
            var type = typeof(T);
            
            if (!resources.TryGetValue(type, out var instance)) return;
            GlobalEvents.UnregisterMultipleUnsafe(instance);

            resources.Remove(type);
            
            recentlyUnassigned.Add(type);
            onAnyResourceUnassigned.Invoke(type);
            if (onUnassignedObservers.TryGetValue(type, out var action)) action.Invoke();
        }

        public bool IsAssigned<T>() where T : ICharacterResource
            => resources.ContainsKey(typeof(T));

        public IEnumerable<ICharacterResource> GetAll() => resources.Values;

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