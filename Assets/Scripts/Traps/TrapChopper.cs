using System.Collections;
using UnityEngine;

public class TrapChopper : AbstractTrap
{
    [SerializeField] private GameObject trapChopper_blade;

    private TrapChopperState trapChopperState = TrapChopperState.IDLE;

    void Awake()
    {
        Init(1.5f, 0.3f, 1.25f, 4);
    }

    protected override IEnumerator InAction()
    {
        if (trapChopperState == TrapChopperState.IDLE)
        {
            trapChopperState = TrapChopperState.CHOPPING;
            trapChopper_blade.GetComponent<TrapChopper_Blade>().State = TrapChopper_Blade.BladeState.CHOPPING;
        }

        yield return 0;
    }

    protected override IEnumerator AfterAction()
    {
        trapChopperState = TrapChopperState.IDLE;
        trapChopper_blade.GetComponent<TrapChopper_Blade>().State = TrapChopper_Blade.BladeState.STOP;

        yield return 0;
    }

    private enum TrapChopperState
    {
        IDLE,
        CHOPPING
    }
}
