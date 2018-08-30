using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Wolfpack
{
    public static class MovementHelper
    {
        public static readonly Dictionary<Line, float> LinePositions = new Dictionary<Line, float>
        {
            { Line.Left, -2.3f },
            { Line.Center, 0f },
            { Line.Right, 2.3f }
        };

        public static Line GetRandomLine() => LinePositions.ElementAt(Random.Range(0, LinePositions.Count)).Key;

        public static Line GetRandomLine(Line previousLine)
        {
            var newLine = previousLine;
            while (newLine == previousLine)
                newLine = GetRandomLine();
            return newLine;
        }
    }

    public enum Line
    {
        Left,
        Center,
        Right
    };
}