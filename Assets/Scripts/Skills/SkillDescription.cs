using System;
using UnityEngine;

namespace Skills
{
    public struct SkillDescription
    {
        public static SkillDescription Empty => new()
        {
            isEmpty = true
        };

        public Sprite icon;
        public string name;
        public string description;
        public bool isEmpty;
    }
}