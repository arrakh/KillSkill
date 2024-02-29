using System;
using UnityEngine;

namespace FlipBooks
{
    [Serializable]
    public class FlipBook
    {
        public FlipBook(string id, float frameRate, bool loop, Sprite[] frames)
        {
            this.id = id;
            this.frameRate = frameRate;
            this.loop = loop;
            this.frames = frames;
        }

        [SerializeField] private string id;
        [SerializeField] private float frameRate;
        [SerializeField] private bool loop;
        [SerializeField] private Sprite[] frames;
        
        public string Id => id;
        public float TotalDuration => frames.Length / frameRate;
        public bool IsLooping => loop;
        
        public Sprite GetFrame(float playTime)
        {
            if (frames == null || frames.Length == 0) return null;

            if (!loop && playTime >= TotalDuration) return frames[^1];
            if (loop) playTime %= TotalDuration;

            int frameIndex = Mathf.FloorToInt((playTime * frameRate) % frames.Length);
            return frames[frameIndex];
        }
        
        public void SetId(string newId) => id = newId;
    }
}