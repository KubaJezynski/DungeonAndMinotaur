using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapArrows : AbstractTrap
{
    private static float ARROW_POSITION_DISTANCE = 1.5f;

    [SerializeField] private GameObject arrow;

    private List<GameObject> arrowSpawns = new List<GameObject>();
    private TrapArrowsState trapArrowsState = TrapArrowsState.READY;

    void Awake()
    {
        Init(3, 0.4f, 2, 3);
        arrowSpawns = CalculateSpawns(ARROW_POSITION_DISTANCE);
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

    protected override IEnumerator InAction()
    {
        if (trapArrowsState == TrapArrowsState.READY)
        {
            trapArrowsState = TrapArrowsState.NOT_READY;
            List<Vector3> shuffledArrowsPositions = new List<Vector3>();
            arrowSpawns.ForEach(arrowSpawn => shuffledArrowsPositions.Add(arrowSpawn.transform.position));
            shuffledArrowsPositions = Shuffle(shuffledArrowsPositions);

            foreach (Vector3 position in shuffledArrowsPositions)
            {
                Instantiate(arrow, position, this.transform.rotation);
                yield return new WaitForSeconds(0.01f);
            }
        }
        else if (trapArrowsState == TrapArrowsState.NOT_READY)
        {
            trapArrowsState = TrapArrowsState.RELOADING;
            yield return new WaitForSeconds(1.5f);
            trapArrowsState = TrapArrowsState.READY;
        }
    }

    protected override IEnumerator AfterAction()
    {
        yield return 0;
    }

    private enum TrapArrowsState
    {
        READY,
        RELOADING,
        NOT_READY
    }
}
