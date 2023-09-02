using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public const int MAX_HEALTH = 100;
    public const int MAX_ENERGY = 1000;

    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject canvas;

    private int health = MAX_HEALTH;
    public int Health { get { return health; } }
    private int energy = MAX_ENERGY;
    public int Energy { get { return energy; } }

    void Awake()
    {
        camera = Instantiate(camera);
        camera.GetComponent<CameraManager>().Player = this.gameObject;
        canvas = Instantiate(canvas);
        canvas.GetComponent<PlayerCanvasManager>().Player = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEnergy();
    }

    private void UpdateEnergy()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            this.gameObject.GetComponent<PlayerMovement>().sprintEvent = () => { if (ConsumptionEnergy()) this.gameObject.GetComponent<PlayerMovement>().sprintEvent = () => false; return true; };
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            this.gameObject.GetComponent<PlayerMovement>().sprintEvent = () => { if (RenewalEnergy()) this.gameObject.GetComponent<PlayerMovement>().sprintEvent = () => false; return false; };
        }
    }

    private bool RenewalEnergy()
    {
        if (energy != MAX_ENERGY)
        {
            energy++;
            energy = energy > MAX_ENERGY ? MAX_ENERGY : energy;
        }

        return energy == MAX_ENERGY;
    }

    private bool ConsumptionEnergy()
    {
        if (energy != 0)
        {
            energy--;
            energy = energy < 0 ? 0 : energy;
        }

        return energy == 0;
    }
}
