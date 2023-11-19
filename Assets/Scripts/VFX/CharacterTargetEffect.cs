using System;
using AllIn1SpriteShader;
using UnityEngine;

namespace VFX
{
    public class CharacterTargetEffect : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        public void SetHighlight(bool on)
        {
            if (on) spriteRenderer.material.EnableKeyword("HOLOGRAM_ON");
            else spriteRenderer.material.DisableKeyword("HOLOGRAM_ON");
        }

        public void SetLockOn(bool on)
        {
            if (on) spriteRenderer.material.EnableKeyword("TWISTUV_ON");
            else spriteRenderer.material.DisableKeyword("TWISTUV_ON");
        }
    }
}