using System;
using UnityEngine;

namespace DefaultNamespace.VFX
{

    public class VfxUnityObject : MonoBehaviour, IVfxObject
    {
        private string key;
        private float timeout;
        
        private Transform toFollow;
        private bool shouldFollow = false;

        public void Follow(Transform transformToFollow)
        {
            shouldFollow = true;
            toFollow = transformToFollow;
        }

        private void Update()
        {
            CheckFollow();
        }

        private void CheckFollow()
        {
            if (shouldFollow) return;
            transform.position = toFollow.position;
            transform.localScale = toFollow.localScale;
            transform.rotation = toFollow.rotation;
        }
    }
}