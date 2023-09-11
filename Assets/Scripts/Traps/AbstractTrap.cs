using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractTrap : MonoBehaviour
{
    private float cooldown = 0;
    private float delay = 0;
    private float duration = 0;
    private int amountOfUse = 0;
    private State state = State.READY;

    void Awake()
    {
        Init(0, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.IN_ACTION)
        {
            StartCoroutine(InAction());
        }
    }

    IEnumerator OnTriggerStay(Collider other)
    {
        if (amountOfUse > 0 && state == State.READY)
        {
            amountOfUse--;
            state = State.ACTIVATED;
            yield return StartCoroutine(ActivateTrap(delay));
            yield return StartCoroutine(TrapInAction(duration));
            yield return StartCoroutine(TrapAfterAction(cooldown));
        }
    }

    protected void Init(float cooldown, float delay, float duration, int amountOfUse)
    {
        this.cooldown = cooldown;
        this.delay = delay;
        this.duration = duration;
        this.amountOfUse = amountOfUse;
    }

    protected abstract IEnumerator InAction();
    protected abstract IEnumerator AfterAction();

    private IEnumerator ActivateTrap(float delay)
    {
        yield return new WaitForSeconds(delay);
        state = State.IN_ACTION;
    }

    private IEnumerator TrapInAction(float duration)
    {
        yield return new WaitForSeconds(duration);
        state = State.AFTER_ACTION;
        StartCoroutine(AfterAction());
    }

    private IEnumerator TrapAfterAction(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        state = State.READY;
    }

    protected List<GameObject> CalculateSpawns(float distance)
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

    private enum State
    {
        READY,
        ACTIVATED,
        IN_ACTION,
        AFTER_ACTION
    }
}
