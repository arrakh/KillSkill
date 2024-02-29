using System;
using Arr.EventsSystem;
using DefaultNamespace.SessionData;
using KillSkill.SessionData.Events;

namespace KillSkill.SessionData
{
    public static class Session
    {
        public static T GetData<T>() where T : ISessionData
        {
            var data = GlobalEvents.Query<ISessionData, QueryDataEvent>(new () {type = typeof(T)});
            if (data == null) throw new Exception($"Could not query type {typeof(T)}, returned null");
            if (data is not T t) throw new Exception($"Could not query type {typeof(T)}, is wrong type!");
            return t;
        }
    }
}