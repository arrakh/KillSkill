using System;
using System.Collections.Generic;
using UnityEngine;

namespace FlipBooks
{
    public class FlipBookPlayer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        private FlipBook currentFlipBook;
        private FlipBook defaultFlipBook;
        private Dictionary<string, FlipBook> collection = new();
        
        private float playSpeed = 1f;
        private float currentTime = 0f;
        private bool shouldCallback = false;
        private bool revertToDefault = true;
        private Action onCurrentDone;
        
        public void Initialize(FlipBook @default, IEnumerable<FlipBook> flipBooks)
        {
            collection.Clear();
            defaultFlipBook = @default;
            collection[defaultFlipBook.Id] = defaultFlipBook;
            foreach (var entry in flipBooks)
                collection[entry.Id.ToLowerInvariant()] = entry;
        }

        public bool Has(string id) => collection.ContainsKey(id);

        public void Play(string id, float speed = 1f, Action onDone = null, bool revertToDefaultOnDone = true)
        {
            if (!collection.TryGetValue(id.ToLowerInvariant(), out currentFlipBook))
            {
                Debug.LogError($"COULD NOT FIND FLIPBOOK ID {id}");
                return;
            }

            playSpeed = speed;
            currentTime = 0;
            shouldCallback = onDone != null;
            onCurrentDone = onDone;
            revertToDefault = revertToDefaultOnDone;
        }

        private void Update()
        {
            if (currentFlipBook == null) return;
            currentTime += Time.deltaTime * playSpeed;
            if (!currentFlipBook.IsLooping && currentTime > currentFlipBook.TotalDuration)
            {
                if (shouldCallback)
                {
                    shouldCallback = false;
                    onCurrentDone?.Invoke();
                }
                else if (revertToDefault) currentFlipBook = defaultFlipBook;
            }
            spriteRenderer.sprite = currentFlipBook.GetFrame(currentTime);
        }
    }
}