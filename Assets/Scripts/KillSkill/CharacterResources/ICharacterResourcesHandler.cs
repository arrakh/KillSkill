using System;
using System.Collections.Generic;
using Arr;
using CharacterResources;

namespace KillSkill.CharacterResources
{
    public interface ICharacterResourcesHandler
    {
        public IReadOnlyDictionary<Type, ICharacterResource> Current { get; }

        public void Assign<T>(T newResource, bool overrideExisting = false) where T : ICharacterResource;
        public void Unassign<T>() where T : ICharacterResource;
        public bool IsAssigned<T>() where T : ICharacterResource;
        public IEnumerable<ICharacterResource> GetAll();
        public T Get<T>() where T : ICharacterResource;
        public bool TryGet<T>(out T instance) where T : ICharacterResource;

        
        
        #region Observe Functions

        public void ObserveUnassigned<T>(Action onUnassigned, bool persistent = true) where T : ICharacterResource;

        public void ObserveAssigned<T>(CharacterResourcesHandler.ResourceAssignedDelegate<T> onAssigned, bool persistent = true)
            where T : ICharacterResource;

        public void ObserveAnyAssigned(EventTemplate<Type, ICharacterResource>.EventHandler onAnyAssigned);

        public void ObserveAnyUnassigned(EventTemplate<Type>.EventHandler onAnyUnassigned);

        #endregion

        
        
        #region Unobserve Functions
        
        public void UnobserveUnassigned<T>(Action onUnassigned) where T : ICharacterResource;

        public void UnobserveAssigned<T>(CharacterResourcesHandler.ResourceAssignedDelegate<T> onAssigned) where T : ICharacterResource;

        public void UnobserveAnyAssigned(EventTemplate<Type, ICharacterResource>.EventHandler onAnyAssigned);

        public void UnobserveAnyUnassigned(EventTemplate<Type>.EventHandler onAnyUnassigned);

        #endregion

    }
}