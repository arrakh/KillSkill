using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KillSkill.SessionData;

namespace KillSkill.Utility
{
    public static class ReflectionUtility
    {
        public static IEnumerable<Type> GetAll<T>(bool includeSelf = false)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(T).IsAssignableFrom(type) && (includeSelf || type != typeof(T)));
        }
    }
}