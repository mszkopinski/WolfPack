using System.Collections.Generic;

namespace Wolfpack
{
    public static class MovementHelper
    {
        public static readonly Dictionary<Line, float> Lines = new Dictionary<Line, float>
        {
            { Line.Left, -2.3f },
            { Line.Center, 0f },
            { Line.Right, 2.3f }
        };
    }

    public enum Line
    {
        Left,
        Center,
        Right
    };
}