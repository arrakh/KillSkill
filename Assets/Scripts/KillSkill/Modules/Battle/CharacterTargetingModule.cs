using System.Threading.Tasks;
using Arr.EventsSystem;
using Arr.ModulesSystem;
using Arr.Utils;
using KillSkill.Characters;
using KillSkill.Modules.Battle.Events;
using KillSkill.VisualEffects;
using UnityEngine;

namespace KillSkill.Modules.Battle
{
    public class CharacterTargetingModule : BaseModule,
        IEventListener<LocalPlayerInitializedEvent>
    {
        private Camera camera;
        private ICharacter lastTarget;
        private ICharacter lastHighlight;
        private CharacterTargetEffect lastTargetEffect;
        private CharacterTargetEffect lastHighlightEffect;

        private bool shouldRun;
        private ICharacter localCharacter;

        protected override Task OnLoad()
        {
            camera = Camera.main;
            UnityEvents.onUpdate += Update;
            return base.OnLoad();
        }

        protected override Task OnUnload()
        {
            UnityEvents.onUpdate -= Update;
            return base.OnUnload();
        }

        private void Update()
        {
            if (!shouldRun) return;
            DetectHighlight();
            DetectTarget();
        }

        private void DetectTarget()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            if (lastHighlight == null) return;
            if (lastTarget == lastHighlight) return;
            if (lastTargetEffect) lastTargetEffect.SetStatus(false, lastTargetEffect.IsHighlighted);
            lastTarget = lastHighlight;
            lastTargetEffect = lastHighlightEffect;
            lastTargetEffect.SetStatus(true, lastTargetEffect.IsHighlighted);

            localCharacter.SetTarget(lastTarget);
        }

        private void DetectHighlight()
        {
            if (!TryRaycastCharacter(out var character))
            {
                if (lastHighlightEffect) lastHighlightEffect.SetStatus(lastHighlightEffect.IsLockedOn, false);
                lastHighlight = null;
                lastHighlightEffect = null;
                return;
            }

            if (lastHighlight == character) return;

            lastHighlight = character;

            if (!lastHighlight.GameObject.TryGetComponent<CharacterTargetEffect>(out var highlighter)) return;
            lastHighlightEffect = highlighter;
            highlighter.SetStatus(lastHighlightEffect.IsLockedOn, true);
        }

        private bool TryRaycastCharacter(out ICharacter character)
        {
            character = null;
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            var raycast = Physics.Raycast(ray, out var hitInfo);
            if (!raycast) return false;
            if (!hitInfo.transform.TryGetComponent(out character)) return false;
            return true;
        }

        public void OnEvent(LocalPlayerInitializedEvent data)
        {
            localCharacter = data.localPlayer;
            localCharacter.OnTargetUpdated.Subscribe(OnLocalTargetUpdated);
            shouldRun = true;
        }

        private void OnLocalTargetUpdated(ICharacter target)
        {
            if (target == lastTarget) return;
            if (lastTarget != null) lastTargetEffect.SetStatus(false, lastTargetEffect.IsHighlighted);
            if (!target.GameObject.TryGetComponent(out lastTargetEffect)) return;

            lastTarget = target;
            lastTargetEffect.SetStatus(true, lastTargetEffect.IsHighlighted);
        }
    }
}