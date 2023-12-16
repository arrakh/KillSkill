using System;
using UnityEngine;

namespace FlipBooks
{
    [CreateAssetMenu(fileName = "NewFlipBook", menuName = "Animation/FlipBook", order = 1)]
    public class FlipBookAsset : ScriptableObject
    {
        public FlipBook flipBook;

        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (flipBook.Id.Equals("NULL_ID")) flipBook.SetId(name);
        }
        #endif
    }
}