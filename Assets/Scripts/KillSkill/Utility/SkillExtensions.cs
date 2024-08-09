using System;
using KillSkill.Skills;

namespace KillSkill.Utility
{
    public static class SkillExtensions
    {
        public static bool IsEmpty(this Skill skill) => skill is null or EmptySkill;
        public static bool IsEmpty(this Type skillType) => skillType == null || skillType == typeof(EmptySkill);
    }
}