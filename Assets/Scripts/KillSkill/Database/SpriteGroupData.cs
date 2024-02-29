using System;
using System.Collections.Generic;
using Arr.SDS;
using UnityEngine;

namespace DefaultNamespace.Database
{
    [Serializable]
    public struct SpriteData
    {
        public string id;
        public Sprite sprite;
    }
    
    [CreateAssetMenu(menuName = "Sprite Group")]
    public class SpriteGroupData : ScriptableObject, IScriptableKey
    {
        [SerializeField] private string groupId;
        [SerializeField] private SpriteData[] sprites;

        public SpriteData[] Data => sprites;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(groupId)) groupId = name;
        }
        #endif
        public string Id => groupId;
    }
}