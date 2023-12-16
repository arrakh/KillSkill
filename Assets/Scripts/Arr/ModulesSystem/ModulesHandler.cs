using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Arr.EventsSystem;
using Unity.VisualScripting;
using UnityEngine;
using EventHandler = Arr.EventsSystem.EventHandler;

namespace Arr.ModulesSystem
{
    public class ModulesHandler
    {
        private Dictionary<Type, IModule> modules;
        private EventHandler eventHandler;

        public ModulesHandler(IEnumerable<IModule> modules, EventHandler eventHandler)
        {
            this.modules = new();
            foreach (var module in modules)
            {
                var type = module.GetType();
                if (this.modules.ContainsKey(type))
                    throw new Exception($"Trying to add duplicate instance of type {type.Name}");
                this.modules[type] = module;
            }
            
            this.eventHandler = eventHandler;
        }

        public async Task Start()
        {
            foreach (var module in modules.Values)
            {
                InjectEvents(module);
                await module.Initialize();
            }

            foreach (var pair in modules)
                InjectDependencies(pair.Key, pair.Value);

            foreach (var module in modules.Values)
                await module.Load();
        }

        private void InjectDependencies(Type moduleType, IModule instance)
        {
            var fields = moduleType.GetFields();

            foreach (var field in fields)
            {
                var attrib = field.GetCustomAttribute(typeof(InjectModuleAttribute));
                if (attrib is not InjectModuleAttribute) continue;
                var type = field.FieldType;

                if (!typeof(IModule).IsAssignableFrom(type))
                    throw new Exception($"Trying to inject type {type.Name} but it is not a Module!");
                
                if (!modules.TryGetValue(type, out var module))
                    throw new Exception($"Trying to inject type {type.Name} but could not find the Module!");
                
                TypedReference tr = __makeref(instance);
                field.SetValueDirect(tr, module);
            }
        }

        private void InjectEvents(IModule module)
        {
            var interfaces = module.GetType().GetInterfaces();
            foreach (var i in interfaces)
            {
                if (!i.IsGenericType) continue;

                var genericType = i.GetGenericTypeDefinition();
                var args = i.GetGenericArguments();
                InvokeEventFunction(nameof(EventHandler.Register), module, genericType, method => method.MakeGenericMethod(args));
            }
        }

        private void InvokeEventFunction(string funcName, IModule module, Type genericEventType, Func<MethodInfo, MethodInfo> makeGenericMethodFunc)
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

            genericMethod.Invoke(eventHandler, new object[] { module });
        }

        private void UnregisterEvents(IModule module)
        {
            var interfaces = module.GetType().GetInterfaces();
            
            foreach (var i in interfaces)
            {
                if (!i.IsGenericType) continue;

                var genericType = i.GetGenericTypeDefinition();
                var args = i.GetGenericArguments();
                InvokeEventFunction(nameof(EventHandler.Unregister), module, genericType, method => method.MakeGenericMethod(args));
            }
        }

        public async Task Stop()
        {
            foreach (var module in modules.Values)
            {
                UnregisterEvents(module);
                await module.Unload();
            }
        }
    }
}