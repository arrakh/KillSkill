using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KillSkill.Utility;
using UnityEngine.Rendering;

namespace KillSkill.Skills
{
    public static class SkillTypeMapper
    {
        private static TypeMapper<Skill> _mapper;

        private static bool _initialized = false;

        public static void Initialize()
        {
            _mapper = new TypeMapper<Skill>();
            _initialized = true;
        }

        public static Type[] ToTypeArray(uint[] indexes)
        {
            if (!_initialized) throw new Exception("Trying to convert index array to Type array but SkillTypeMapping is NOT initialized");
            return _mapper.ToTypeArray(indexes);
        }

        public static uint[] ToIdArray(Type[] skillTypes)
        {
            if (!_initialized) throw new Exception("Trying to convert Type array to index array but SkillTypeMapping is NOT initialized");
            return _mapper.ToIdArray(skillTypes);
        }
    }
}