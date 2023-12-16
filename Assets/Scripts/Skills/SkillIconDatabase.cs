using System;
using Arr.SDS;
using UnityEngine;

namespace Skills
{
    [Serializable]
    public class SkillIconData : IScriptableKey<string>
    {
        public string key;
        public Sprite icon;
        public string Key => key;
    }
    
    [CreateAssetMenu(menuName = "Skill Icon DB")]
    public class SkillIconDatabase : PairScriptableDatabase<string, SkillIconData>
    {
        
    }
}