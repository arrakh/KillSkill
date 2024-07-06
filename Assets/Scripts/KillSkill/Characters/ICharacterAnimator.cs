using System;
using DG.Tweening;
using UnityEngine;

namespace KillSkill.Characters
{
    public interface ICharacterAnimator
    {
        public Transform Visual { get; }
        
        public SpriteRenderer Sprite { get; }

        public void UpdatePosition(Vector3 newPosition);

        public void PlayFlipBook(string id, float speed = 1f, Action onDone = null, bool revertToDefaultOnDone = true);

        public bool HasFlipBook(string id);

        public void Damage(float intensity);

        public void BackToPosition();

        public void AddTweens(params Tween[] t);

        public void AddMovementTweens(params Tween[] t);
    }
}