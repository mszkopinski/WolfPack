using UnityEngine;

namespace Wolfpack
{
    public static class TransformExtensions
    {
        public static void ChangePosition(this Transform transform, string coordinate, float value)
        {
            coordinate = coordinate.ToLower();
            var newPosition = transform.position;
            
            if (coordinate == "x")
                newPosition.z = value;
            else if (coordinate == "y")
                newPosition.y = value;
            else if (coordinate == "z")
                newPosition.z = value;
            
            transform.position = newPosition;
        }
    }
}