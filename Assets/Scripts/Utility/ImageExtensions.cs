using UnityEngine;

namespace Wolfpack
{
    public static class ImageExtensions
    {
        public static void SetColorAlpha(this Color color, float alpha)
        {
            var newColor = color;
            newColor.a = alpha;
            color = newColor;
        }
    }
}