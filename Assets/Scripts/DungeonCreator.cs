using System.Collections.Generic;
using CustomMath;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    [SerializeField] private GameObject dungeonRoomTypeManagerPrefab;
    [SerializeField] private GameObject trapTypeManagerPrefab;

    private DungeonDataStruct dungeonData;
    private DungeonRoomType roomType;
    private TrapType trapType;
    private DungeonRoomStruct startingRoom = new DungeonRoomStruct();
    private DungeonRoomStruct endingRoom = new DungeonRoomStruct();
    private List<DungeonRoomStruct> emptyRooms = new List<DungeonRoomStruct>();
    private List<DungeonRoomStruct> safeRooms = new List<DungeonRoomStruct>();
    private List<DungeonRoomStruct> otherRooms = new List<DungeonRoomStruct>();
    private List<DungeonRoomStruct> extremeRooms = new List<DungeonRoomStruct>();
    private List<DungeonRoomStruct> notExtremeRooms = new List<DungeonRoomStruct>();
    private List<GameObject> floors = new List<GameObject>();
    private List<GameObject> walls = new List<GameObject>();
    private List<GameObject> stairs = new List<GameObject>();
    private List<GameObject> traps = new List<GameObject>();

    void Awake()
    {
        InitializeDungeonData();
        roomType = Instantiate(dungeonRoomTypeManagerPrefab).GetComponent<DungeonRoomType>();
        roomType.Set(this.dungeonData.floorType);
        trapType = Instantiate(trapTypeManagerPrefab).GetComponent<TrapType>();
        CreateSpace(this.dungeonData.dungeonSize, roomType);

        BuildDungeon();
    }

    private void InitializeDungeonData()
    {
        this.dungeonData = GameManager.Instance.dungeonDataHandler;
    }

    // Create rooms structs
    private void CreateSpace(int size, DungeonRoomType roomType)
    {
        int safePathStepsCount = this.dungeonData.safePathLength;
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
                Vector3 newPosition = MathFunctions.CalculateNewPosition(currentRoom, currentAngle, currentRoom.type.diameter);

                if (MathFunctions.FindIndexWithTreshold(this.emptyRooms, newPosition, distanceTreshold) < 0)
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
                Vector3 newPosition = MathFunctions.CalculateNewPosition(safePathLastRoom, currentAngle, safePathLastRoom.type.diameter);

                if (MathFunctions.FindIndexWithTreshold(this.emptyRooms, newPosition, distanceTreshold) < 0)
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
                    Vector3 newPosition = MathFunctions.CalculateNewPosition(iLoopRooms[j], currentAngle, iLoopRooms[j].type.diameter);

                    if (MathFunctions.FindIndexWithTreshold(this.emptyRooms, newPosition, distanceTreshold) < 0)
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
            int index = MathFunctions.FindIndexWithTreshold(emptyRooms, otherRoom.position, distanceTreshold);

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
        }

        BuildStairs(endingRoom);

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
            else if (!(MathFunctions.FindIndexWithTreshold(safeRooms, floor.transform.position, roomType.diameter * 0.99f) < 0))
            {
                floor.GetComponent<Renderer>().material.color = new Color(255, 255, 0);
            }
        }

        // Divide rooms into extreme and not

        DivideRoomsIntoExtremeAndNot(this.emptyRooms, this.extremeRooms, this.notExtremeRooms);

        // Build walls at the extreme positions of rooms and ending room
        float distanceTreshold = roomType.diameter * 0.1f;

        foreach (DungeonRoomStruct room in extremeRooms)
        {
            for (int i = 0; i < roomType.cornersCount; i++)
            {
                Vector3 newPosition = MathFunctions.CalculateNewPosition(room, i * room.type.cornerAngle, room.type.diameter);

                if (MathFunctions.FindIndexWithTreshold(emptyRooms, newPosition, distanceTreshold) < 0)
                {
                    float angle = MathFunctions.CalculateAngle(room.position, newPosition);
                    walls.Add(Instantiate(roomType.wall, MathFunctions.CalculateNewPosition(room, i * room.type.cornerAngle, room.type.diameter / 2f), Quaternion.Euler(new Vector3(0, 0, -angle))));
                }
            }
        }

        BuildTraps();

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

    private void BuildTraps()
    {
        int trapTypesLength = this.dungeonData.traps.Count;

        if (trapTypesLength == 0)
        {
            return;
        }

        int nTrapsToInstantiate = Random.Range(this.otherRooms.Count / 4, this.otherRooms.Count / 2);
        List<DungeonRoomStruct> rooms = new List<DungeonRoomStruct>();
        this.otherRooms.ForEach(room => rooms.Add(room));

        for (int i = 0; i < nTrapsToInstantiate; i++)
        {
            int roomIndex = Random.Range(0, rooms.Count);
            DungeonRoomStruct room = rooms[roomIndex];
            TrapType.Type trapTypeType = this.dungeonData.traps[Random.Range(0, trapTypesLength)];
            this.trapType.SetTrap(trapTypeType);
            traps.Add(trapType.InstantiateTrap(room, this.extremeRooms, this.walls));
            rooms.RemoveAt(roomIndex);
        }
    }

    private void DungeonCreatedCallback()
    {
        GameManager.Instance.DungeonCreatedEvent = (GameObject player) => Instantiate(player, startingRoom.position, Quaternion.identity);
    }


    private float CalculateNearestAngle(float angle, DungeonRoomStruct room)
    {
        List<Vector3> emptyRoomsPositions = new List<Vector3>();
        emptyRooms.ForEach(room => emptyRoomsPositions.Add(room.position));
        float nearestAngle = 360f;
        float angleDifference = 360f;

        for (int i = 0; i < room.type.cornersCount; i++)
        {
            Vector3 newPosition = MathFunctions.CalculateNewPosition(room, i * room.type.cornerAngle, room.type.diameter);

            if (MathFunctions.ContainsWithTreshold(emptyRoomsPositions, newPosition, room.type.diameter / 10f))
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
        int index = MathFunctions.FindIndexWithTreshold(emptyRooms, nextRoomPosition, startingRoom.type.diameter / 10f);

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
            Vector3 newPosition = MathFunctions.CalculateNewPosition(room, i * room.type.cornerAngle, room.type.diameter);

            if (MathFunctions.FindIndexWithTreshold(emptyRooms, newPosition, distanceTreshold) < 0)
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
}
