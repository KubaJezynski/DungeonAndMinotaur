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
    [SerializeField] private GameObject floorHeptagonalPrefab;
    [SerializeField] private GameObject floorOctagonalPrefab;

    public float diameter { get; private set; }
    public int cornersCount { get; private set; }
    public float cornerAngle { get; private set; }
    public GameObject stair { get; private set; }
    public GameObject wall { get; private set; }
    public GameObject floor { get; private set; }

    void Awake()
    {
        Set(FloorType.OCTAGONAL, SIDE_LENGTH);
    }

    public void Set(FloorType floorType)
    {
        Set(floorType, SIDE_LENGTH);
    }

    private void Set(FloorType floorType, int sideLength)
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
            case FloorType.HEPTAGONAL:
                r = sideLength / (2 * Mathf.Tan(Mathf.PI / 7));
                SetRoom(r * 2, 7, stairPrefab, wallPrefab, floorHeptagonalPrefab);
                break;
            case FloorType.OCTAGONAL:
                r = sideLength * (1 + Mathf.Sqrt(2)) / 2;
                SetRoom(r * 2, 8, stairPrefab, wallPrefab, floorOctagonalPrefab);
                break;
        }
    }

    private void SetRoom(float diameter, int cornersCount, GameObject stair, GameObject wall, GameObject floor)
    {
        this.diameter = diameter;
        this.cornersCount = cornersCount;
        this.cornerAngle = 360f / cornersCount;
        this.stair = stair;
        this.wall = wall;
        this.floor = floor;
    }

    public enum FloorType
    {
        TRIANGULAR,
        QUADRANGULAR,
        PENTAGONAL,
        HEXAGONAL,
        HEPTAGONAL,
        OCTAGONAL
    }
}
