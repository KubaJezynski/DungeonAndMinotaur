using System.Collections.Generic;

public readonly struct DungeonDataStruct
{
    public int safePathLength { get; }
    public int dungeonSize { get; }
    public DungeonRoomType.FloorType floorType { get; }
    public List<TrapType.Type> traps { get; }

    public DungeonDataStruct(int safePathLength, int dungeonSize, DungeonRoomType.FloorType floorType, List<TrapType.Type> traps)
    {
        this.safePathLength = safePathLength;
        this.dungeonSize = dungeonSize;
        this.floorType = floorType;
        this.traps = traps;
    }
}
