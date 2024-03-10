using System;
using System.Collections.Generic;
using System.Linq;
using Arr;
using CharacterResources;

namespace KillSkill.CharacterResources
{
    public partial class CharacterResourcesHandler : IDisposable
    {
        private Dictionary<Type, ICharacterResource> resources = new();

        private HashSet<Type> recentlyUnassigned = new();
        private Dictionary<Type, IUpdatableCharacterResource> updatableResources = new();

        public IReadOnlyDictionary<Type, ICharacterResource> Current => resources;


        public void Update(float deltaTime)
        {
            foreach (var resource in updatableResources.Values.ToList())
                resource.OnUpdate(deltaTime);
        }

        public void Assign<T>(T newResource, bool overrideExisting = false) where T : ICharacterResource
        {
            var type = typeof(T);
            if (!overrideExisting && resources.ContainsKey(type))
                throw new Exception($"Resource of type {type} already exist!");

            recentlyUnassigned.Remove(type);

            resources[type] = newResource;

            if (newResource is IUpdatableCharacterResource updatable) 
                updatableResources[type] = updatable;
            
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
            updatableResources.Remove(type);
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