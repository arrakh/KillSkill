using System;
using UnityEngine;

namespace Skills
{
    [Serializable]
    public struct SkillMetadata
    {
        public static SkillMetadata Empty => new()
        {
            isEmpty = true
        };

        public Sprite icon;
        public string name;
        public string description;
        public string extraDescription;
        public bool isEmpty;
    }
}