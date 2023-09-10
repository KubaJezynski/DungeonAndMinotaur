using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapArrows : AbstractTrap
{
    private static float ARROW_POSITION_DISTANCE = 1.5f;

    [SerializeField] private GameObject arrow;

    private List<GameObject> arrowSpawns = new List<GameObject>();
    private bool canActivate = true;
    private bool waitToActivate = false;

    void Awake()
    {
        Init(3, 0.4f, 2, 3);
        arrowSpawns = CalculateArrowSpawns(ARROW_POSITION_DISTANCE);
    }

    private List<GameObject> CalculateArrowSpawns(float distance)
    {
        List<GameObject> arrowSpawns = new List<GameObject>();
        float length = this.transform.localScale.z;
        int positionsCount = (int)(length / distance);
        distance = length / (positionsCount + 1);
        float rotation = this.transform.rotation.eulerAngles.z;
        Vector3 startingPosition = new Vector3(this.transform.position.x + Mathf.Cos(Mathf.Deg2Rad * -rotation) * this.transform.localScale.x / 2f,
                                                this.transform.position.y + Mathf.Sin(Mathf.Deg2Rad * -rotation) * this.transform.localScale.x / 2f,
                                                this.transform.position.z - this.transform.localScale.z / 2f);

        for (int i = 0; i < positionsCount; i++)
        {
            for (int j = 0; j < positionsCount; j++)
            {
                GameObject arrowSpawn = new GameObject();
                float cosX = Mathf.Cos(Mathf.Deg2Rad * -rotation) * distance * (j + 1);
                float sinY = Mathf.Sin(Mathf.Deg2Rad * -rotation) * distance * (j + 1);
                arrowSpawn.transform.position = new Vector3(startingPosition.x - cosX, startingPosition.y - sinY, startingPosition.z + distance * (i + 1));
                arrowSpawn.transform.rotation = this.transform.rotation;
                arrowSpawn.transform.parent = this.transform;
                arrowSpawns.Add(arrowSpawn);
            }
        }

        return arrowSpawns;
    }

    private List<Vector3> Shuffle(List<Vector3> list)
    {
        List<Vector3> tempList = new List<Vector3>();
        List<Vector3> shuffledList = new List<Vector3>();

        list.ForEach(item => tempList.Add(item));

        for (int i = 0; i < tempList.Count; i++)
        {
            int index = Random.Range(0, tempList.Count);
            Vector3 item = tempList[index];
            tempList.Remove(item);
            shuffledList.Add(item);
        }

        return shuffledList;
    }

    protected override IEnumerator OnActivated()
    {
        if (canActivate)
        {
            canActivate = false;
            List<Vector3> shuffledArrowsPositions = new List<Vector3>();
            arrowSpawns.ForEach(arrowSpawn => shuffledArrowsPositions.Add(arrowSpawn.transform.position));
            shuffledArrowsPositions = Shuffle(shuffledArrowsPositions);

            foreach (Vector3 position in shuffledArrowsPositions)
            {
                Instantiate(arrow, position, this.transform.rotation);
                yield return new WaitForSeconds(0.01f);
            }
        }

        if (!canActivate && !waitToActivate)
        {
            waitToActivate = true;
            yield return new WaitForSeconds(1.5f);
            waitToActivate = false;
            canActivate = true;
        }
    }
}
