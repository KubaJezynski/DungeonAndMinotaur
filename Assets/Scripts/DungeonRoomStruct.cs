using UnityEngine;

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