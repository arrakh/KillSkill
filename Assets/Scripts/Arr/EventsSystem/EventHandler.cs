using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arr.EventsSystem
{
    public class EventHandler
    {
        private readonly Dictionary<Type, List<Delegate>> eventListeners = new();

        public void Register<TParam>(IEventListener<TParam> listener) where TParam : struct
        {
            var eventType = typeof(TParam);
            if (!eventListeners.ContainsKey(eventType)) eventListeners[eventType] = new List<Delegate>();
            eventListeners[eventType].Add(new Action<TParam>(listener.OnEvent));
        }

        public void Fire<TParam>(TParam data) where TParam : struct
        {
            var eventType = typeof(TParam);
            if (!eventListeners.TryGetValue(eventType, out var listener)) return;
            foreach (var listenerDelegate in listener)
                ((Action<TParam>) listenerDelegate)?.Invoke(data);
        }
        
        
        public void Unregister<TParam>(IEventListener<TParam> listener) where TParam : struct
        {
            var eventType = typeof(TParam);
            Action<TParam> targetMethod = listener.OnEvent;

            if (!eventListeners.TryGetValue(eventType, out var listenerList)) return;
            listenerList.RemoveAll(del => del.Target == listener && del.Method.Equals(targetMethod.Method));
            
            if (listenerList.Count == 0) eventListeners.Remove(eventType);
        }
        
        private readonly Dictionary<Type, IQueryProvider<object>> queryListeners = new ();
        private readonly Dictionary<Type, object> paramQueryListeners = new ();

        public void Register<TReturn>(IQueryProvider<TReturn> provider)
        {
            var queryType = typeof(TReturn);

            if (queryListeners.ContainsKey(queryType))
                throw new InvalidOperationException($"A listener for query type {queryType.Name} is already registered.");
    
            queryListeners[queryType] = provider as IQueryProvider<object>;
        }

        public void Register<TReturn, TParam>(IQueryProvider<TReturn, TParam> provider) where TParam : struct
        {
            var queryType = typeof(Tuple<TReturn, TParam>);

            if (paramQueryListeners.ContainsKey(queryType))
                throw new InvalidOperationException($"A listener for query type {queryType.Name} with parameter {typeof(TParam).Name} is already registered.");
    
            paramQueryListeners[queryType] = provider;
        }

        public TReturn Query<TReturn>()
        {
            var queryType = typeof(TReturn);
            if (queryListeners.TryGetValue(queryType, out var listener) && listener is IQueryProvider<TReturn> queryListener)
                return queryListener.OnQuery();
            return default;
        }

        public TReturn Query<TReturn, TParam>(TParam data) where TParam : struct
        {
            Debug.Log($"REGISTERED QUERY OF TYPE RETURN {typeof(TReturn)}, PARAM {typeof(TParam)}");
            var queryType = typeof(Tuple<TReturn, TParam>);
            if (paramQueryListeners.TryGetValue(queryType, out var listener) && listener is IQueryProvider<TReturn, TParam> paramQueryListener)
                return paramQueryListener.OnQuery(data);
            return default;
        }

        //This is made to be consistent with the signature of the register and for reflection purposes
        public void Unregister<TReturn>(IQueryProvider<TReturn> provider) => Unregister<TReturn>();
        public void Unregister<TReturn, TParam>(IQueryProvider<TReturn, TParam> provider) where TParam : struct
            => Unregister<TReturn, TParam>();
        
        public void Unregister<TReturn>()
        {
            var queryType = typeof(TReturn);
            queryListeners.Remove(queryType);
        }

        public void Unregister<TReturn, TParam>() where TParam : struct
        {
            Debug.Log($"UNREGISTERED QUERY OF TYPE RETURN {typeof(TReturn)}, PARAM {typeof(TParam)}");
            var queryType = typeof(Tuple<TReturn, TParam>);
            paramQueryListeners.Remove(queryType);
        }
    }
}