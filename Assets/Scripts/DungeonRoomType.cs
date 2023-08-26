using UnityEngine;

public class DungeonRoomType : MonoBehaviour
{
    private const int SIDE_LENGTH = 5;

    [SerializeField] private GameObject stairPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject floorTriangularPrefab;
    [SerializeField] private GameObject floorQuadrangularPrefab;
    [SerializeField] private GameObject floorPentagonalPrefab;
    [SerializeField] private GameObject floorHexagonalPrefab;

    public float diameter { get; private set; }
    public int cornersCount { get; private set; }
    public int cornerAngle { get; private set; }
    public GameObject stair { get; private set; }
    public GameObject wall { get; private set; }
    public GameObject floor { get; private set; }

    void Awake()
    {
        Create(FloorType.HEXAGONAL, SIDE_LENGTH);
    }

    private void Create(FloorType floorType, int sideLength)
    {
        float r;

        switch (floorType)
        {
            case FloorType.TRIANGULAR:
                r = sideLength * Mathf.Sqrt(3) / 6f;
                SetRoom(r * 2, 3, stairPrefab, wallPrefab, floorTriangularPrefab);
                break;
            case FloorType.QUADRANGULAR:
                SetRoom(sideLength, 4, stairPrefab, wallPrefab, floorQuadrangularPrefab);
                break;
            case FloorType.PENTAGONAL:
                r = sideLength * Mathf.Sqrt(25 + 10 * Mathf.Sqrt(5)) / 10f;
                SetRoom(r * 2, 5, stairPrefab, wallPrefab, floorPentagonalPrefab);
                break;
            case FloorType.HEXAGONAL:
                r = sideLength * Mathf.Sqrt(3) / 2f;
                SetRoom(r * 2, 6, stairPrefab, wallPrefab, floorHexagonalPrefab);
                break;
        }
    }

    private void SetRoom(float diameter, int cornersCount, GameObject stair, GameObject wall, GameObject floor)
    {
        this.diameter = diameter;
        this.cornersCount = cornersCount;
        this.cornerAngle = 360 / cornersCount;
        this.stair = stair;
        this.wall = wall;
        this.floor = floor;
    }

    public enum FloorType
    {
        TRIANGULAR,
        QUADRANGULAR,
        PENTAGONAL,
        HEXAGONAL
    }
}
