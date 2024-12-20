﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arr.EventsSystem;
using Arr.ModulesSystem;
using KillSkill.SessionData.Events;
using KillSkill.Utility;

namespace KillSkill.SessionData
{
    public class SessionDataModule : BaseModule, IOrderedModule,
        IQueryProvider<ISessionData, QueryDataEvent>
    {
        public int Order => int.MinValue;
        
        private Dictionary<Type, ISessionData> sessionData = new();

        protected override async Task OnInitialize()
        {
            var dataTypes = ReflectionCache.GetAll<ISessionData>();

            foreach (var type in dataTypes)
            {
                var instance = Activator.CreateInstance(type) as ISessionData;
                if (instance == null) throw new Exception($"Trying to initialize {type} but instance is null");
                if (instance is ILoadableSessionData loadable) loadable.OnLoad();
                sessionData[type] = instance;
            }
        }

        protected override async Task OnUnload()
        {
            foreach (var (_, data) in sessionData)
            {
                if (data is ILoadableSessionData loadable) loadable.OnUnload();
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