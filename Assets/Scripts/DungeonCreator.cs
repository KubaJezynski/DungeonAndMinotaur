using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    private const int DUNGEON_SIZE = 5;
    private const int ROOM_QUADRANGULAR_SIDE_LENGTH = 5;
    private Vector3[,] roomQuadrangularPositions = CalculateSpace(DUNGEON_SIZE, ROOM_QUADRANGULAR_SIDE_LENGTH);

    [SerializeField] private GameObject roomQuadrangular;
    [SerializeField] private GameObject roomQuadrangular_wall_1_3;
    [SerializeField] private GameObject roomQuadrangular_wall_2_3_4;
    [SerializeField] private GameObject roomQuadrangular_wall_3;
    [SerializeField] private GameObject roomQuadrangular_wall_3_4;
    private GameObject[] roomQuadrangulars;

    void Awake()
    {
        roomQuadrangulars = new [] {roomQuadrangular, roomQuadrangular_wall_1_3, roomQuadrangular_wall_2_3_4, roomQuadrangular_wall_3, roomQuadrangular_wall_3_4};

        // First instantiate starting room at position zero
        Instantiate(roomQuadrangulars[0], roomQuadrangularPositions[DUNGEON_SIZE, DUNGEON_SIZE], Quaternion.identity);

        // (test) Instantiate random rooms around starting room
        int dungeonLength = roomQuadrangularPositions.GetLength(0);

        for (int i = 0; i < dungeonLength; i++)
        {
            for (int j = 0; j < dungeonLength; j++)
            {
                if (i == DUNGEON_SIZE && j == DUNGEON_SIZE)
                {
                    continue;
                }
                Instantiate(roomQuadrangulars[Random.Range(1, roomQuadrangulars.Length)], roomQuadrangularPositions[i, j], Quaternion.identity);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private static Vector3[,] CalculateSpace(int size, int distance)
    {
        int n = size*2+1;
        Vector3[,] result = new Vector3[n, n];
        int x = -size * distance;
        int y = -size * distance;

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                result[i, j] = new Vector3(x + i * distance, y + j * distance, 0);
            }
        }

        return result;
    }
}
