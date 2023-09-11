using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpikes : AbstractTrap
{
    private static float SPIKE_POSITION_DISTANCE = 1.5f;

    [SerializeField] private GameObject spike;

    private List<GameObject> spikeSpawns = new List<GameObject>();
    private List<GameObject> spikes = new List<GameObject>();
    private TrapSpikesState trapSpikesState = TrapSpikesState.HIDDEN;

    void Awake()
    {
        Init(4, 0.3f, 1.5f, 5);
        spikeSpawns = CalculateSpawns(SPIKE_POSITION_DISTANCE);
    }

    protected override IEnumerator InAction()
    {
        if (trapSpikesState == TrapSpikesState.HIDDEN)
        {
            trapSpikesState = TrapSpikesState.EXTENDED;

            foreach (GameObject spikeSpawn in spikeSpawns)
            {
                spikes.Add(Instantiate(spike, spikeSpawn.transform.position, this.transform.rotation));
            }

            foreach (GameObject spike in spikes)
            {
                spike.GetComponent<Spike>().State = Spike.SpikeState.EXTENDING;
            }
        }

        yield return 0;
    }

    protected override IEnumerator AfterAction()
    {
        yield return trapSpikesState = TrapSpikesState.HIDDEN;

        foreach (GameObject spike in spikes)
        {
            spike.GetComponent<Spike>().State = Spike.SpikeState.HIDING;
        }

        spikes.Clear();
    }

    private enum TrapSpikesState
    {
        HIDDEN,
        EXTENDED
    }
}
