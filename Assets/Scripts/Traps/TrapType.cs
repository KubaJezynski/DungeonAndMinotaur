using System.Collections.Generic;
using UnityEngine;

public class TrapType : MonoBehaviour
{
    [SerializeField] private GameObject trapArrowsPrefab;
    [SerializeField] private GameObject trapSpikesPrefab;

    public GameObject trap { get; private set; }

    void Awake()
    {
        SetTrap(trapArrowsPrefab);
    }

    public void SetTrap(Type type)
    {
        switch (type)
        {
            case Type.ARROWS:
                SetTrap(trapArrowsPrefab);
                break;
            case Type.SPIKES:
                SetTrap(trapSpikesPrefab);
                break;
        }
    }

    private void SetTrap(GameObject trap)
    {
        this.trap = trap;
    }

    public GameObject InstantiateTrap(DungeonRoomStruct room, List<DungeonRoomStruct> extremeRooms, List<GameObject> walls)
    {
        List<TrapLocation> trapLocations = new List<TrapLocation>();
        trapLocations.Add(TrapLocation.FLOOR);
        int extremeRoomIndex = CustomMath.MathFunctions.FindIndexWithTreshold(extremeRooms, room.position, 0.1f);

        if (extremeRoomIndex > -1)
        {
            trapLocations.Add(TrapLocation.WALL);
        }

        switch (trapLocations[Random.Range(0, trapLocations.Count)])
        {
            case TrapLocation.WALL:
                return InstantiateTrapOnWall(room, walls);
            case TrapLocation.FLOOR:
                return InstantiateTrapOnFloor(room);
        }

        return null;
    }

    private GameObject InstantiateTrapOnWall(DungeonRoomStruct room, List<GameObject> walls)
    {
        float distanceTreshold = room.type.diameter * 0.1f;
        float rotationAngle = room.type.cornerAngle * Random.Range(0, room.type.cornersCount);

        for (int i = 0; i < room.type.cornersCount; i++)
        {
            Vector3 newPosition = CustomMath.MathFunctions.CalculateNewPosition(room, i * room.type.cornerAngle + rotationAngle, room.type.diameter / 2);

            if (CustomMath.MathFunctions.FindWithTreshold(walls, newPosition, distanceTreshold) != null)
            {
                GameObject trap = Instantiate(this.trap);
                Vector3 trapPosition = CustomMath.MathFunctions.CalculateNewPosition(room, i * room.type.cornerAngle + rotationAngle, room.type.diameter / 2 - room.type.wall.transform.localScale.y / 2);
                float trapAngle = CustomMath.MathFunctions.CalculateAngle(trapPosition, room.position);
                trap.transform.position = trapPosition;
                trap.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -trapAngle));

                return trap;
            }
        }

        return null;
    }

    private GameObject InstantiateTrapOnFloor(DungeonRoomStruct room)
    {
        GameObject trap = Instantiate(this.trap);
        Vector3 trapPosition = new Vector3(room.position.x, room.position.y, room.position.z + room.type.wall.transform.localScale.z / 2 - room.type.floor.transform.localScale.z / 2);
        trap.transform.position = trapPosition;
        trap.transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
        return trap;
    }

    public enum Type
    {
        ARROWS,
        SPIKES
    }

    public enum TrapLocation
    {
        WALL,
        FLOOR
    }
}
