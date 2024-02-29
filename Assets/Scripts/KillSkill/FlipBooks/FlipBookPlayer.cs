using System;
using System.Collections.Generic;
using UnityEngine;

namespace FlipBooks
{
    public class FlipBookPlayer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private FlipBookAsset[] flipBooks;
        [SerializeField] private FlipBookAsset defaultFlipBook;
        
        private FlipBook flipBook;
        private Dictionary<string, FlipBook> collection = new();
        private float playSpeed = 1f;
        private float currentTime = 0f;
        private bool shouldCallback = false;
        private Action onCurrentDone;
        
        public void Initialize()
        {
            foreach (var entry in flipBooks)
                collection[entry.flipBook.Id.ToLowerInvariant()] = entry.flipBook;
        }

        public bool Has(string id) => collection.ContainsKey(id);

        public void Play(string id, float speed = 1f, Action onDone = null)
        {
            if (!collection.TryGetValue(id.ToLowerInvariant(), out flipBook))
            {
                Debug.LogError($"COULD NOT FIND FLIPBOOK ID {id}");
                return;
            }

            playSpeed = speed;
            currentTime = 0;
            shouldCallback = onDone != null;
            onCurrentDone = onDone;
        }

        private void Update()
        {
            if (flipBook == null) return;
            currentTime += Time.deltaTime * playSpeed;
            if (!flipBook.IsLooping && currentTime > flipBook.TotalDuration)
            {
                if (shouldCallback)
                {
                    shouldCallback = false;
                    onCurrentDone?.Invoke();
                }
                else flipBook = defaultFlipBook.flipBook;
            }
            spriteRenderer.sprite = flipBook.GetFrame(currentTime);
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (defaultFlipBook == null && flipBooks.Length > 0) defaultFlipBook = flipBooks[0];
        }
        #endif
    }
}