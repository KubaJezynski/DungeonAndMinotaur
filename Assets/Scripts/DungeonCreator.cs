using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    private const int DUNGEON_SIZE = 5;
    private const int ROOM_QUADRANGULAR_SIDE_LENGTH = 5;

    [SerializeField] private GameObject roomQuadrangular;
    [SerializeField] private GameObject roomQuadrangular_wall_1_3;
    [SerializeField] private GameObject roomQuadrangular_wall_2_3_4;
    [SerializeField] private GameObject roomQuadrangular_wall_3;
    [SerializeField] private GameObject roomQuadrangular_wall_3_4;
    private GameObject[] roomQuadrangulars;
    private Vector3[,] roomQuadrangularPositions = CalculateSpace(DUNGEON_SIZE, ROOM_QUADRANGULAR_SIDE_LENGTH);

    void Awake()
    {
        roomQuadrangulars = new[] { roomQuadrangular, roomQuadrangular_wall_1_3, roomQuadrangular_wall_2_3_4, roomQuadrangular_wall_3, roomQuadrangular_wall_3_4 };

        // (test) Instantiate random rooms around starting room
        int dungeonLength = roomQuadrangularPositions.GetLength(0);

        /*for (int i = 0; i < dungeonLength; i++)
        {
            for (int j = 0; j < dungeonLength; j++)
            {
                if (i == DUNGEON_SIZE && j == DUNGEON_SIZE)
                {
                    continue;
                }
                Instantiate(roomQuadrangulars[Random.Range(1, roomQuadrangulars.Length)], roomQuadrangularPositions[i, j], Quaternion.identity);
            }
        }*/
        buildDungeon();
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
        int n = size * 2 + 1;
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

    private void buildDungeon()
    {
        // First instantiate starting room at position zero
        Instantiate(roomQuadrangulars[0], roomQuadrangularPositions[DUNGEON_SIZE, DUNGEON_SIZE], Quaternion.identity);

        // Choose exit position and instantiate starting room
        int index = Random.Range(0, 2) == 0 ? 0 : roomQuadrangularPositions.GetLength(0) - 1;
        Vector3 exitPosition = Random.Range(0, 2) == 0 ? roomQuadrangularPositions[index, Random.Range(0, roomQuadrangularPositions.GetLength(1))] :
                                                        roomQuadrangularPositions[Random.Range(0, roomQuadrangularPositions.GetLength(0)), index];
        Instantiate(roomQuadrangulars[0], exitPosition, Quaternion.identity);

        // Build rooms with walls around edge of dungeon
        int dungeonLength = roomQuadrangularPositions.GetLength(0);

        for (int i = 0; i < dungeonLength; i++)
        {
            for (int j = 0; j < dungeonLength; j++)
            {
                Vector3 pos0 = roomQuadrangularPositions[DUNGEON_SIZE, DUNGEON_SIZE];
                Vector3 pos1 = roomQuadrangularPositions[i, j];
                int angle = (int)calculateAngle(pos0, pos1);

                if ((i > 0 && i < dungeonLength - 1) && (j > 0 && j < dungeonLength - 1))
                {
                    continue;
                }
                else
                {
                    if (pos1 == exitPosition)
                    {
                        continue;
                    }

                    if (angle % 45 == 0 && angle % 90 != 0)
                    {
                        Instantiate(roomQuadrangular_wall_3_4, roomQuadrangularPositions[i, j], Quaternion.Euler(new Vector3(0, 0, -angle - 135)));
                    }
                    else
                    {
                        angle = angle > 45 && angle < 135 ? 90 :
                                angle > 135 || angle < -135 ? 0 :
                                angle < -45 && angle > -135 ? -90 : 180;

                        Instantiate(roomQuadrangular_wall_3, roomQuadrangularPositions[i, j], Quaternion.Euler(new Vector3(0, 0, angle)));
                    }
                }
            }
        }
    }

    private float calculateAngle(Vector2 p1, Vector2 p2)
    {
        float y = p2.y - p1.y;
        float x = p2.x - p1.x;

        return Mathf.Atan2(x, y) * Mathf.Rad2Deg;
    }
}
