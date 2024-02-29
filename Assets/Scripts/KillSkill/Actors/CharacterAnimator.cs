using System;
using System.Collections.Generic;
using DG.Tweening;
using FlipBooks;
using UnityEngine;

namespace Actors
{
    public class CharacterAnimator : MonoBehaviour
    {
        [SerializeField] private Transform visualTransform;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private FlipBookPlayer flipBookPlayer;

        public Transform Visual => visualTransform;
        public SpriteRenderer Sprite => spriteRenderer;

        private List<Tween> tweens = new();
        private Vector3 originalPosition;

        public void Initialize()
        {
            originalPosition = visualTransform.position;
            flipBookPlayer.Initialize();
        }

        public void PlayFlipBook(string id, float speed = 1f, Action onDone = null) => flipBookPlayer.Play(id, speed, onDone);

        public bool HasFlipBook(string id) => flipBookPlayer.Has(id);

        //SHOULD BE AN ANIMATION ENTRY
        public void Damage(float intensity)
        {
            spriteRenderer.color = new Color(1f, 0.5f, 0.5f, 1f);
            Tween color = spriteRenderer.DOColor(Color.white, 0.33f);
            Tween shake = visualTransform.DOShakePosition(0.33f, Vector3.right * intensity);
            AddTweens(color, shake);
            flipBookPlayer.Play("damaged");
        }

        public void BackToPosition()
        {
            Tween move = visualTransform.DOMove(originalPosition, 0.33f).SetEase(Ease.OutQuart);
            AddTweens(move);
        }

        public void AddTweens(params Tween[] t)
        {
            tweens.AddRange(t);
        }

        private void Clear()
        {
            foreach (var t in tweens)
                t.Kill(true);
            
            BackToPosition();
        }
    }
}