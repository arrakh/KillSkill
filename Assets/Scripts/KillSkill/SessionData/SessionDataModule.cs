using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arr.EventsSystem;
using Arr.ModulesSystem;
using KillSkill.SessionData;
using KillSkill.SessionData.Events;
using KillSkill.Utility;
using UnityEngine;

namespace DefaultNamespace.SessionData
{
    public class SessionDataModule : BaseModule, 
        IQueryProvider<ISessionData, QueryDataEvent>
    {
        private Dictionary<Type, ISessionData> sessionData = new();

        protected override async Task OnInitialize()
        {
            var dataTypes = ReflectionUtility.GetAll<ISessionData>();

            foreach (var type in dataTypes)
            {
                var instance = Activator.CreateInstance(type) as ISessionData;
                if (instance == null) throw new Exception($"Trying to initialize {type} but instance is null");
                instance.OnLoad();
                GlobalEvents.Instance.RegisterMultiple(instance);
                sessionData[type] = instance;
            }
        }

        protected override async Task OnUnload()
        {
            foreach (var (_, data) in sessionData)
            {
                GlobalEvents.Instance.UnregisterMultiple(data);
                data.OnUnload();
            }
            
            sessionData.Clear();
        }

        public ISessionData OnQuery(QueryDataEvent data)
        {
            if (sessionData.TryGetValue(data.type, out var d)) return d;
            return null;
        }
    }
}