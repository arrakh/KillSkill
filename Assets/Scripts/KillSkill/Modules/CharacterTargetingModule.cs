using System;
using KillSkill.Characters;
using UnityEngine;
using VFX;

namespace Systems
{
    public class CharacterTargetingModule : MonoBehaviour
    {
        [SerializeField] private Character player;

        private Camera camera;
        private Character lastTarget = null;
        private Character lastHighlight = null;
        private CharacterTargetEffect lastTargetEffect = null;
        private CharacterTargetEffect lastHighlightEffect = null;
        
        private void Start()
        {
            camera = Camera.main;
        }

        private void Update()
        {
            DetectHighlight();
            DetectTarget();
        }

        private void DetectTarget()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            if (lastHighlight == null) return;
            if (lastTarget == lastHighlight) return;
            if (lastTargetEffect) lastTargetEffect.SetLockOn(false);
            lastTarget = lastHighlight;
            lastTargetEffect = lastHighlightEffect;
            lastTargetEffect.SetLockOn(true);
        }

        private void DetectHighlight()
        {
            if (!TryRaycastCharacter(out var character))
            {
                if (lastHighlightEffect) lastHighlightEffect.SetHighlight(false);
                lastHighlight = null;
                lastHighlightEffect = null;
                return;
            }

            if (lastHighlight == character) return;

            lastHighlight = character;
            
            if (!lastHighlight.TryGetComponent<CharacterTargetEffect>(out var highlighter)) return;
            lastHighlightEffect = highlighter;
            highlighter.SetHighlight(true);
        }

        private bool TryRaycastCharacter(out Character character)
        {
            character = null;
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            var raycast = Physics.Raycast(ray, out var hitInfo);
            if (!raycast) return false;
            if (!hitInfo.transform.TryGetComponent(out character)) return false;
            return true;
        }
    }
}