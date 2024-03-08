using System.Collections.Generic;
using Arr.SDS;
using UnityEngine;

namespace KillSkill.Database
{
    public static class SpriteDatabase
    {
        public static Sprite Get(string id) => SpriteGroupDatabase.GetSprite(id);
    }
    
    [CreateAssetMenu(menuName = "Scriptable DBs/Sprite Group DB")]
    public class SpriteGroupDatabase : ObjectScriptableDatabase<SpriteGroupData>
    {
        private static Dictionary<string, Sprite> _spriteMap = new();

        protected override void OnAllDataRegistered()
        {
            _spriteMap = new();

            foreach (var group in data)
                foreach (var spriteData in group.Data)
                    if (!_spriteMap.ContainsKey(spriteData.id)) _spriteMap[spriteData.id] = spriteData.sprite;
                    else Debug.LogWarning($"Registering {spriteData.id} from group {group.Id} but duplicate is found!"); 
        }

        public static Sprite GetSprite(string id)
            => !_spriteMap.TryGetValue(id, out var sprite) ? null : sprite;
    }
}