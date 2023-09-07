using System.Collections.Generic;
using CustomMath;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    private const int DUNGEON_SIZE = 10;

    [SerializeField] private GameObject dungeonRoomTypePrefab;

    private DungeonRoomType roomType;
    private DungeonRoomStruct startingRoom = new DungeonRoomStruct();
    private DungeonRoomStruct endingRoom = new DungeonRoomStruct();
    private List<DungeonRoomStruct> emptyRooms = new List<DungeonRoomStruct>();
    private List<DungeonRoomStruct> safeRooms = new List<DungeonRoomStruct>();
    private List<DungeonRoomStruct> otherRooms = new List<DungeonRoomStruct>();
    private List<GameObject> floors = new List<GameObject>();
    private List<GameObject> walls = new List<GameObject>();
    private List<GameObject> stairs = new List<GameObject>();

    void Awake()
    {
        roomType = Instantiate(dungeonRoomTypePrefab).GetComponent<DungeonRoomType>();
        CreateSpace(DUNGEON_SIZE, roomType);

        BuildDungeon();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Create rooms structs
    private void CreateSpace(int size, DungeonRoomType roomType)
    {
        int safePathStepsCount = Random.Range(size * size / 2, size * size);
        float distanceTreshold = roomType.diameter * 0.99f;
        float halfCornerAngle = roomType.cornerAngle / 2f;
        bool isOddCorner = roomType.cornersCount % 2 == 0 ? false : true;

        // Starting room
        this.startingRoom = new DungeonRoomStruct(Vector3.zero, new Quaternion(), roomType);
        this.emptyRooms.Add(startingRoom);

        // Safe path rooms
        float baseAngle = roomType.cornerAngle * Random.Range(0, roomType.cornersCount);

        for (int i = 0; i < safePathStepsCount; i++)
        {
            DungeonRoomStruct currentRoom = emptyRooms[emptyRooms.Count - 1];
            float currentAngle = baseAngle + roomType.cornerAngle * Random.Range(0, roomType.cornersCount - 1);

            for (int j = 0; j < currentRoom.type.cornersCount; j++)
            {
                Vector3 newPosition = CalculateNextPosition(currentRoom, currentAngle);

                if (FindIndexWithTreshold(this.emptyRooms, newPosition, distanceTreshold) < 0)
                {
                    float safePathRoomAngle = MathFunctions.CalculateAngle(newPosition, currentRoom.position);
                    DungeonRoomStruct newRoom = new DungeonRoomStruct(newPosition, Quaternion.Euler(new Vector3(0, 0, -safePathRoomAngle)), roomType);
                    this.emptyRooms.Add(newRoom);
                    this.safeRooms.Add(newRoom);
                    break;
                }

                currentAngle = -currentAngle;
            }
        }

        // Ending room
        for (int i = 0; i < safeRooms.Count; i++)
        {
            bool endingRoomCreated = false;
            DungeonRoomStruct safePathLastRoom = safeRooms[safeRooms.Count - 1 - i];

            for (int j = 0; j < safePathLastRoom.type.cornersCount; j++)
            {
                float currentAngle = safePathLastRoom.type.cornerAngle * j;
                Vector3 newPosition = CalculateNextPosition(safePathLastRoom, currentAngle);

                if (FindIndexWithTreshold(this.emptyRooms, newPosition, distanceTreshold) < 0)
                {
                    float endingRoomAngle = MathFunctions.CalculateAngle(newPosition, safePathLastRoom.position);
                    DungeonRoomStruct newRoom = new DungeonRoomStruct(newPosition, Quaternion.Euler(new Vector3(0, 0, -endingRoomAngle)), roomType);
                    this.emptyRooms.Add(newRoom);
                    this.endingRoom = newRoom;
                    endingRoomCreated = true;
                    break;
                }
            }

            if (endingRoomCreated)
            {
                break;
            }
        }

        // Other rooms
        List<DungeonRoomStruct> iLoopRooms = new List<DungeonRoomStruct>();
        emptyRooms.ForEach(room => iLoopRooms.Add(room));

        for (int i = 0; i < size; i++)
        {
            List<DungeonRoomStruct> newRooms = new List<DungeonRoomStruct>();

            for (int j = 0; j < iLoopRooms.Count; j++)
            {
                for (int k = 0; k < roomType.cornersCount; k++)
                {
                    float currentAngle = k * iLoopRooms[j].type.cornerAngle;
                    Vector3 newPosition = CalculateNextPosition(iLoopRooms[j], currentAngle);

                    if (FindIndexWithTreshold(this.emptyRooms, newPosition, distanceTreshold) < 0)
                    {
                        float otherRoomAngle = MathFunctions.CalculateAngle(newPosition, iLoopRooms[j].position);
                        DungeonRoomStruct newEmptyRoom = new DungeonRoomStruct(newPosition, Quaternion.Euler(new Vector3(0, 0, -otherRoomAngle)), roomType);
                        newRooms.Add(newEmptyRoom);
                        emptyRooms.Add(newEmptyRoom);
                        otherRooms.Add(newEmptyRoom);
                    }
                }
            }

            iLoopRooms = newRooms;
        }

        // Remove some other rooms
        int nRoomsToRemove = Random.Range(otherRooms.Count / 4, otherRooms.Count / 3);

        for (int i = 0; i < nRoomsToRemove; i++)
        {
            DungeonRoomStruct otherRoom = otherRooms[Random.Range(0, otherRooms.Count)];
            int index = FindIndexWithTreshold(emptyRooms, otherRoom.position, distanceTreshold);

            if (!(index < 0))
            {
                emptyRooms.RemoveAt(index);
                otherRooms.Remove(otherRoom);
            }
        }
    }

    private void BuildDungeon()
    {
        // Build floors and stairs
        foreach (DungeonRoomStruct room in emptyRooms)
        {
            floors.Add(Instantiate(room.type.floor, new Vector3(room.position.x, room.position.y, room.position.z + room.type.wall.transform.localScale.z / 2f), room.rotation));

            if (room.Equals(endingRoom))
            {
                BuildStairs(endingRoom);
            }
        }

        /* Just color starting room on green, ending room on red, safe path on yellow, delete later */
        foreach (GameObject floor in floors)
        {
            if (floor.transform.position == new Vector3(startingRoom.position.x, startingRoom.position.y, floor.transform.position.z))
            {
                floor.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
            }
            else if (floor.transform.position == new Vector3(endingRoom.position.x, endingRoom.position.y, floor.transform.position.z))
            {
                floor.GetComponent<Renderer>().material.color = new Color(255, 0, 0);
            }
            else if (!(FindIndexWithTreshold(safeRooms, floor.transform.position, roomType.diameter * 0.99f) < 0))
            {
                floor.GetComponent<Renderer>().material.color = new Color(255, 255, 0);
            }
        }

        // Divide rooms into extreme and not
        List<DungeonRoomStruct> extremeRooms = new List<DungeonRoomStruct>();
        List<DungeonRoomStruct> notExtremeRooms = new List<DungeonRoomStruct>();

        DivideRoomsIntoExtremeAndNot(emptyRooms, extremeRooms, notExtremeRooms);

        Debug.Log("extremeRooms = " + extremeRooms.Count);
        Debug.Log("notExtremeRooms = " + notExtremeRooms.Count);

        // Build walls at the extreme positions of rooms and ending room
        float distanceTreshold = roomType.diameter * 0.1f;

        foreach (DungeonRoomStruct room in extremeRooms)
        {
            for (int i = 0; i < roomType.cornersCount; i++)
            {
                Vector3 newPosition = CalculateNextPosition(room, i * room.type.cornerAngle);

                if (FindIndexWithTreshold(emptyRooms, newPosition, distanceTreshold) < 0)
                {
                    float angle = MathFunctions.CalculateAngle(room.position, newPosition);
                    walls.Add(Instantiate(roomType.wall, CalculateNextPosition(room, i * room.type.cornerAngle, room.type.diameter / 2f), Quaternion.Euler(new Vector3(0, 0, -angle))));
                }
            }
        }

        DungeonCreatedCallback();
    }

    private void BuildStairs(DungeonRoomStruct room)
    {
        GameObject stairs = new GameObject("Stairs");
        stairs.transform.position = room.position;
        stairs.transform.Rotate(new Vector3(0, 0, room.rotation.eulerAngles.z + 180));
        float stairsRotation = stairs.transform.rotation.eulerAngles.z;
        Vector3 stairsStartPosition = new Vector3(room.position.x + Mathf.Sin(Mathf.Deg2Rad * -(stairsRotation + 180)) * room.type.diameter / 2f,
                                                room.position.y + Mathf.Cos(Mathf.Deg2Rad * -(stairsRotation + 180)) * room.type.diameter / 2f,
                                                room.position.z + room.type.wall.transform.localScale.z / 2f);
        float nStairsToInstantiate = 10f;
        float stairLength = room.type.diameter / nStairsToInstantiate;
        float halfStairLength = room.type.stair.transform.localScale.y / 2f;
        float stairHeight = room.type.wall.transform.localScale.z / nStairsToInstantiate;
        float sinX = Mathf.Sin(Mathf.Deg2Rad * -stairsRotation);
        float cosY = Mathf.Cos(Mathf.Deg2Rad * -stairsRotation);

        for (int i = 1; i < nStairsToInstantiate; i++)
        {
            float stairDistance = stairLength * i;
            Vector3 stairPosition = new Vector3(stairsStartPosition.x + sinX * stairDistance + sinX * halfStairLength,
                                                stairsStartPosition.y + cosY * stairDistance + cosY * halfStairLength,
                                                stairsStartPosition.z - stairHeight * i);
            GameObject stair = Instantiate(room.type.stair, stairPosition, Quaternion.Euler(0, 0, stairs.transform.rotation.eulerAngles.z));
            stair.transform.parent = stairs.transform;
        }

        this.stairs.Add(stairs);
    }

    private void DungeonCreatedCallback()
    {
        GameObject gameManager = GameObject.FindWithTag("GameManager");

        if (gameManager != null)
        {
            gameManager.GetComponent<GameManager>().dungeonCreatedEvent = (GameObject player) => Instantiate(player, startingRoom.position, Quaternion.identity);
        }
    }

    private Vector3 CalculateNextPosition(DungeonRoomStruct room, float rotationAngle)
    {
        return CalculateNextPosition(room, rotationAngle, room.type.diameter);
    }

    private Vector3 CalculateNextPosition(DungeonRoomStruct room, float rotationAngle, float distance)
    {
        return MathFunctions.CalculateNewPosition(room.position, rotationAngle + room.rotation.eulerAngles.z, distance);
    }

    private float CalculateNearestAngle(float angle, DungeonRoomStruct room)
    {
        List<Vector3> emptyRoomsPositions = new List<Vector3>();
        emptyRooms.ForEach(room => emptyRoomsPositions.Add(room.position));
        float nearestAngle = 360f;
        float angleDifference = 360f;

        for (int i = 0; i < room.type.cornersCount; i++)
        {
            Vector3 newPosition = CalculateNextPosition(room, i * room.type.cornerAngle);

            if (ContainsWithTreshold(emptyRoomsPositions, newPosition, room.type.diameter / 10f))
            {
                float newAngle = MathFunctions.CalculateAngle(room.position, newPosition);
                float newAngleDifference = Mathf.Abs(angle - newAngle);

                if (newAngleDifference < angleDifference)
                {
                    nearestAngle = newAngle;
                    angleDifference = newAngleDifference;
                }
            }
        }

        return nearestAngle;
    }

    private DungeonRoomStruct CalculateNextRoom(DungeonRoomStruct startingRoom, DungeonRoomStruct endingRoom)
    {
        float turnAroundAngle = MathFunctions.CalculateAngle(startingRoom.position, endingRoom.position);
        turnAroundAngle = CalculateNearestAngle(turnAroundAngle, startingRoom);
        Vector3 nextRoomPosition = new Vector3(startingRoom.position.x + Mathf.Sin(Mathf.Deg2Rad * turnAroundAngle) * startingRoom.type.diameter,
                                                startingRoom.position.y + Mathf.Cos(Mathf.Deg2Rad * turnAroundAngle) * startingRoom.type.diameter,
                                                startingRoom.position.z);
        int index = FindIndexWithTreshold(emptyRooms, nextRoomPosition, startingRoom.type.diameter / 10f);

        return index < 0 ? startingRoom : emptyRooms[index];
    }

    private int CalculateDistanceSteps(DungeonRoomStruct startingRoom, DungeonRoomStruct endingRoom)
    {
        DungeonRoomStruct currentRoom = startingRoom;
        int distanceSteps = 0;

        for (int i = 0; i < emptyRooms.Count; i++)
        {
            currentRoom = CalculateNextRoom(currentRoom, endingRoom);

            if (endingRoom.position.Equals(currentRoom.position))
            {
                break;
            }

            distanceSteps++;
        }

        Debug.Log("distanceSteps = " + distanceSteps);
        return distanceSteps;
    }

    private bool IsExtremeRoom(DungeonRoomStruct room)
    {
        float distanceTreshold = room.type.diameter * 0.1f;

        for (int i = 0; i < room.type.cornersCount; i++)
        {
            Vector3 newPosition = CalculateNextPosition(room, i * room.type.cornerAngle);

            if (FindIndexWithTreshold(emptyRooms, newPosition, distanceTreshold) < 0)
            {
                return true;
            }
        }

        return false;
    }

    private void DivideRoomsIntoExtremeAndNot(List<DungeonRoomStruct> rooms, List<DungeonRoomStruct> extremeRooms, List<DungeonRoomStruct> notExtremeRooms)
    {
        extremeRooms.Clear();
        notExtremeRooms.Clear();

        foreach (DungeonRoomStruct room in emptyRooms)
        {
            if (IsExtremeRoom(room))
            {
                extremeRooms.Add(room);
            }
            else
            {
                notExtremeRooms.Add(room);
            }
        }
    }

    private static bool CalculateDistanceWithTreshold(Vector2 p1, Vector2 p2, float treshold)
    {
        float distance = Vector3.Distance(p1, p2);

        return distance < treshold;
    }

    private static bool ContainsWithTreshold(List<Vector3> positions, Vector3 position, float treshold)
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

    private int FindIndexWithTreshold(List<DungeonRoomStruct> rooms, Vector3 position, float treshold)
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

    private GameObject FindWithTreshold(List<GameObject> gameObjects, Vector3 position, float treshold)
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

    public readonly struct DungeonRoomStruct
    {
        public Vector3 position { get; }
        public Quaternion rotation { get; }
        public DungeonRoomType type { get; }

        public DungeonRoomStruct(Vector3 position, Quaternion rotation, DungeonRoomType type)
        {
            this.position = position;
            this.rotation = rotation;
            this.type = type;
        }
    }
}
