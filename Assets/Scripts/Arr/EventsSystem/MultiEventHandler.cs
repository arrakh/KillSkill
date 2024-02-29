using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Arr.EventsSystem
{
    public partial class EventHandler
    {
        public void RegisterMultiple(object listener)
        {
            var interfaces = listener.GetType().GetInterfaces();
            foreach (var i in interfaces)
            {
                if (!i.IsGenericType) continue;

                var genericType = i.GetGenericTypeDefinition();
                var args = i.GetGenericArguments();
                InvokeEventFunction(nameof(Register), listener, genericType, method => method.MakeGenericMethod(args));
            }
        }

        private void InvokeEventFunction(string funcName, object listener, Type genericEventType, Func<MethodInfo, MethodInfo> makeGenericMethodFunc)
        {
            var method = typeof(EventHandler)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(m => m.IsGenericMethod && m.Name == funcName
                                                       && m.GetParameters().Length == 1
                                                       && m.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == genericEventType);
                             
            if (method == null)
            {
                Debug.LogError($"Failed to find the Register method of type {genericEventType}.");
                return;
            }
            
            var genericMethod = makeGenericMethodFunc.Invoke(method);

            genericMethod.Invoke(this, new object[] { listener });
        }

        public void UnregisterMultiple(object listener)
        {
            var interfaces = listener.GetType().GetInterfaces();
            
            foreach (var i in interfaces)
            {
                if (!i.IsGenericType) continue;

                var genericType = i.GetGenericTypeDefinition();
                var args = i.GetGenericArguments();
                InvokeEventFunction(nameof(Unregister), listener, genericType, method => method.MakeGenericMethod(args));
            }
        }
    }
}