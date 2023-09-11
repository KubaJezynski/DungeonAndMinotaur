using UnityEngine;

public class TrapChopper_Blade : MonoBehaviour
{
    private static float ROTATION_SPEED = 2.5f;
    private static int DAMAGE = 8;

    private BladeState state = BladeState.IDLE;
    public BladeState State
    {
        set
        {
            state = value;

            if (state == BladeState.IDLE)
            {
                actionEvent = () => { };
            }
            else if (state == BladeState.CHOPPING)
            {
                actionEvent = () =>
                {
                    rotationEvent.Invoke();
                };
            }
            else if (state == BladeState.STOP)
            {
                actionEvent = () =>
                {
                    this.transform.localEulerAngles -= new Vector3(0, 0, ROTATION_SPEED);
                    float angle = this.transform.localEulerAngles.z;
                    angle = (angle > 180) ? angle - 360 : angle;

                    if (angle < 0)
                    {
                        this.transform.localEulerAngles = Vector3.zero;
                        RotationState = BladeRotationState.LEFT;
                        State = BladeState.IDLE;
                    }
                };
            }
        }
    }
    private BladeRotationState rotationState = BladeRotationState.LEFT;
    private BladeRotationState RotationState
    {
        set
        {
            rotationState = value;

            if (rotationState == BladeRotationState.LEFT)
            {
                rotationEvent = () =>
                {
                    this.transform.localEulerAngles += new Vector3(0, 0, ROTATION_SPEED);

                    if (this.transform.localEulerAngles.z > 180)
                    {
                        this.transform.localEulerAngles = new Vector3(0, 0, 180);
                        RotationState = BladeRotationState.RIGHT;
                    }
                };
            }
            else if (rotationState == BladeRotationState.RIGHT)
            {
                rotationEvent = () =>
                {
                    this.transform.localEulerAngles -= new Vector3(0, 0, ROTATION_SPEED);
                    float angle = this.transform.localEulerAngles.z;
                    angle = (angle > 180) ? angle - 360 : angle;

                    if (angle < 0)
                    {
                        this.transform.localEulerAngles = Vector3.zero;
                        RotationState = BladeRotationState.LEFT;
                    }
                };
            }
        }
    }
    private System.Action actionEvent { get; set; }
    private System.Action rotationEvent { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        State = BladeState.IDLE;
        RotationState = BladeRotationState.LEFT;
    }

    // Update is called once per frame
    void Update()
    {
        actionEvent.Invoke();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            other.gameObject.GetComponentInParent<PlayerManager>().takeDamageEvent = () => DAMAGE;
        }
    }

    public enum BladeState
    {
        IDLE,
        CHOPPING,
        STOP
    }

    private enum BladeRotationState
    {
        LEFT,
        RIGHT
    }
}
