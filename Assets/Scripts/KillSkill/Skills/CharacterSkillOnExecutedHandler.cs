using System;
using System.Collections.Generic;
using System.Reflection;
using KillSkill.Characters;

namespace KillSkill.Skills
{
    public partial class CharacterSkillHandler
    {
        Dictionary<Type, List<MethodInfo>> skillExecutedCallbacks = new Dictionary<Type, List<MethodInfo>>();

        void DetectAndRegisterOnExecutedCallbacks(Type skillType)
        {
            foreach (Type interfaceType in skillType.GetInterfaces())
            {
                if (!interfaceType.IsGenericType ||
                    interfaceType.GetGenericTypeDefinition() != typeof(ISkillExecutedCallback<>)) continue;
                
                Type targetType = interfaceType.GetGenericArguments()[0]; // The T in ISkillExecutedCallback<T>

                MethodInfo methodInfo = interfaceType.GetMethod("OnExecuted");
                if (methodInfo == null) continue;
                
                if (!skillExecutedCallbacks.ContainsKey(targetType))
                    skillExecutedCallbacks[targetType] = new List<MethodInfo>();
                
                skillExecutedCallbacks[targetType].Add(methodInfo);
            }
        }

        void InvokeCallbacks(Character caster, Character target, Skill targetSkill)
        {
            Type targetSkillType = targetSkill.GetType();

            if (!skillExecutedCallbacks.TryGetValue(targetSkillType, out List<MethodInfo> methodInfos)) return;
            
            foreach (var methodInfo in methodInfos)
                methodInfo.Invoke(targetSkill, new object[] { caster, target, targetSkill });
        }
    }
}