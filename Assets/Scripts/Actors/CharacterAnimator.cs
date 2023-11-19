using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace Actors
{
    public class CharacterAnimator : MonoBehaviour
    {
        [FormerlySerializedAs("characterTransform")] [SerializeField] private Transform visualTransform;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private GameObject testPrefab;
        private ObjectPool<GameObject> testPool;

        public Transform Visual => visualTransform;
        public SpriteRenderer Sprite => spriteRenderer;

        private List<Tween> tweens = new();
        private Vector3 originalPosition;

        private void Awake()
        {
            originalPosition = visualTransform.position;
            testPool = new ObjectPool<GameObject>(TestCreateFunc, OnGet);
        }

        private void OnGet(GameObject obj)
        {
            obj.SetActive(false);
        }

        private GameObject TestCreateFunc()
        {
            var go = Instantiate(testPrefab);
            go.SetActive(false);
            return go;
        }

        public void Damage(float intensity)
        {
            spriteRenderer.color = new Color(1f, 0.5f, 0.5f, 1f);
            Tween color = spriteRenderer.DOColor(Color.white, 0.33f);
            Tween shake = visualTransform.DOShakePosition(0.33f, Vector3.right * intensity);
            AddTweens(color, shake);

            var effect = testPool.Get();
            effect.SetActive(true);
            effect.transform.position = Visual.position;
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