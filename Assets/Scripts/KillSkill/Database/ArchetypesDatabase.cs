using System;
using Arr.SDS;
using UnityEngine;

namespace KillSkill.Database
{
    [Serializable]
    public class ArchetypeData : IScriptableKey<string>
    {
        public string id;
        public string name;
        [TextArea] public string description;
        public Color bannerColor;
        public Color contentColor;
        public string Key => id;
    }
    
    [CreateAssetMenu(menuName = "Archetype DB")]
    public class ArchetypesDatabase : PairScriptableDatabase<string, ArchetypeData>
    {
        
    }
}