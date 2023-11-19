using System;
using System.Collections.Generic;
using Actors;
namespace CharacterResources
{
    public class CharacterResourcesHandler : IDisposable
    {
        private Character character;

        public delegate void ResourceAssignedDelegate<T>(T resource) where T : ICharacterResource;
        
        private Dictionary<Type, Action> onUnassignedObservers = new();
        private Dictionary<Type, Delegate> onAssignedObservers = new();

        private Dictionary<Type, ICharacterResource> resources = new();

        private HashSet<Type> recentlyUnassigned = new();

        public void Assign(ICharacterResource newResource, bool overrideExisting = false)
        {
            var type = newResource.GetType();
            if (!overrideExisting && resources.ContainsKey(type))
                throw new Exception($"Resource of type {type} already exist!");

            recentlyUnassigned.Remove(type);

            resources[type] = newResource;
            if (onAssignedObservers.TryGetValue(type, out var delegateObj))
            {
                var action = delegateObj as ResourceAssignedDelegate<ICharacterResource>;
                action?.Invoke(newResource);
            }
        }

        public void Unassign<T>() where T : ICharacterResource
        {
            var type = typeof(T);
            bool success = resources.Remove(type);
            if (!success) return;
            recentlyUnassigned.Add(type);
            if (onUnassignedObservers.TryGetValue(type, out var action)) action.Invoke();
        }

        public bool IsAssigned<T>() where T : ICharacterResource
            => resources.ContainsKey(typeof(T)); 

        public bool HasAny<T>() where T : ICharacterResource
        {
            if (!resources.TryGetValue(typeof(T), out var resource)) return false;
            return resource.HasAny();
        }
        
        public bool TryGet<T>(out double value) where T : ICharacterResource
        {
            value = default;
            if (!resources.TryGetValue(typeof(T), out var resource)) return false;
            value = resource.Get();
            return true;
        }

        public bool TrySet<T>(double newValue, Character instigator = null) where T : ICharacterResource
        {
            var type = typeof(T);
            if (!resources.TryGetValue(type, out var resource)) return false;
            return resource.TrySet(newValue, instigator);
        }

        public bool TryAdd<T>(double delta, Character instigator = null) where T : ICharacterResource
        {
            var type = typeof(T);
            if (!resources.TryGetValue(typeof(T), out var resource)) return false;
            return resource.TryAdd(delta, instigator);
        }

        public void ObserveUnassigned<T>(Action onUnassigned, bool persistent = true) where T : ICharacterResource
        {
            var type = typeof(T);
            if (persistent && recentlyUnassigned.Contains(type)) onUnassigned.Invoke();
            
            onUnassignedObservers[type] += onUnassigned;
        }

        public void ObserveAssigned<T>(Action<T> onAssigned, bool persistent = true) where T : ICharacterResource
        {
            var type = typeof(T);
            if (persistent && resources.TryGetValue(type, out var resource)) 
                onAssigned.Invoke((T)resource);

            if (!onAssignedObservers.ContainsKey(type))
                onAssignedObservers[type] = null;
            onAssignedObservers[type] = Delegate.Combine(onAssignedObservers[type], onAssigned);
        }
        
        private bool disposed;
        public void Dispose()
        {
            if (disposed) return;
            onUnassignedObservers.Clear();
            onAssignedObservers.Clear();
            disposed = true;
        }
    }
}