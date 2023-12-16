using UnityEngine;

namespace Arr
{
    public static class ColorExtensions
    {
        public static Color Alpha(this Color c, float alpha) => new Color(c.r, c.g, c.b, alpha);
    }
}