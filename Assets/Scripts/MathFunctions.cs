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

        //
        // Summary:
        //     Return new position from position.
        //
        // Parameters:
        //   position:
        //     position from which new position will be calculated.
        //   translationAngle:
        //     world angle in which new position is to be calculated (-180 to 180).
        //   distance:
        //     how far new position should be calculated.
        public static Vector3 CalculateNewPosition(Vector3 position, float translationAngle, float distance)
        {
            return new Vector3(position.x + Mathf.Sin(Mathf.Deg2Rad * translationAngle) * distance,
                                position.y + Mathf.Cos(Mathf.Deg2Rad * translationAngle) * distance,
                                position.z);
        }
    }
}
