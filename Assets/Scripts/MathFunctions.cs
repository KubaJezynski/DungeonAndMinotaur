using System.Collections.Generic;
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

        //
        // Summary:
        //     Return new position from room position.
        //
        // Parameters:
        //   room:
        //     room position from which new position will be calculated.
        //   rotationAngle:
        //     rotate by angle of room corner.
        //   distance:
        //     how far new position should be calculated.
        public static Vector3 CalculateNewPosition(DungeonRoomStruct room, float rotationAngle, float distance)
        {
            return CalculateNewPosition(room.position, rotationAngle + room.rotation.eulerAngles.z, distance);
        }

        //
        // Summary:
        //     Return true if distance is smaller than treshold.
        //
        // Parameters:
        //   p1:
        //     first position.
        //   p2:
        //     second position.
        //   treshold:
        //     treshold.
        public static bool CalculateDistanceWithTreshold(Vector2 p1, Vector2 p2, float treshold)
        {
            float distance = Vector3.Distance(p1, p2);

            return distance < treshold;
        }

        //
        // Summary:
        //     Return true if list contain position with distance smaller then treshold.
        //     Return false if not found.
        //
        // Parameters:
        //   positions:
        //     list of positions.
        //   position:
        //     position to find in list.
        //   treshold:
        //     treshold.
        public static bool ContainsWithTreshold(List<Vector3> positions, Vector3 position, float treshold)
        {
            foreach (Vector3 p in positions)
            {
                if (CalculateDistanceWithTreshold(p, position, treshold))
                {
                    return true;
                }
            }

            return false;
        }

        //
        // Summary:
        //     Return index of room that contain position with distance smaller then treshold.
        //     Return -1 if not found.
        //
        // Parameters:
        //   rooms:
        //     list of rooms.
        //   position:
        //     position to find in list.
        //   treshold:
        //     treshold.
        public static int FindIndexWithTreshold(List<DungeonRoomStruct> rooms, Vector3 position, float treshold)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (CalculateDistanceWithTreshold(rooms[i].position, position, treshold))
                {
                    return i;
                }
            }

            return -1;
        }

        //
        // Summary:
        //     Return game object that contain position with distance smaller then treshold.
        //     Return null if not found.
        //
        // Parameters:
        //   gameObjects:
        //     list of game objects.
        //   position:
        //     position to find in list.
        //   treshold:
        //     treshold.
        public static GameObject FindWithTreshold(List<GameObject> gameObjects, Vector3 position, float treshold)
        {
            foreach (GameObject go in gameObjects)
            {
                if (CalculateDistanceWithTreshold(go.transform.position, position, treshold))
                {
                    return go;
                }
            }

            return null;
        }
    }
}
