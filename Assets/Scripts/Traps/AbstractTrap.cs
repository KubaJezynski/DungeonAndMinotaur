using System.Collections;
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
            StartCoroutine(OnActivated());
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

    protected abstract IEnumerator OnActivated();

    private IEnumerator ActivateTrap(float delay)
    {
        yield return new WaitForSeconds(delay);
        state = State.IN_ACTION;
    }

    private IEnumerator TrapInAction(float duration)
    {
        yield return new WaitForSeconds(duration);
        state = State.AFTER_ACTION;
    }

    private IEnumerator TrapAfterAction(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        state = State.READY;
    }

    private enum State
    {
        READY,
        ACTIVATED,
        IN_ACTION,
        AFTER_ACTION
    }
}
