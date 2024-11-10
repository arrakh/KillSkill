using UnityEngine;

namespace KillSkill.Utility
{
    public class Range
    {
        public readonly float min, max;


        public Range(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public float GetRandom() => Random.Range(min, max);
        public float GetRandomRounded() => Mathf.Round(Random.Range(min, max));

        public override string ToString()
        {
            return $"{min} - {max}";
        }
    }
}