using UnityEngine;

namespace CustomMath
{
    public struct MathFunctions
    {
        //
        // Summary:
        //     Return angle from -180 to 180.
        public static float CalculateAngle(Vector2 p1, Vector2 p2)
        {
            float y = p2.y - p1.y;
            float x = p2.x - p1.x;

            return Mathf.Atan2(x, y) * Mathf.Rad2Deg;
        }
    }
}
