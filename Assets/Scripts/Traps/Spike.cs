using UnityEngine;

public class Spike : MonoBehaviour
{
    private static float SPEED = 0.1f;
    private static int DAMAGE = 12;

    private SpikeState state = SpikeState.IDLE;
    public SpikeState State
    {
        set
        {
            state = value;

            if (state == SpikeState.IDLE)
            {
                actionEvent = () => { };
            }
            else if (state == SpikeState.HIDING)
            {
                actionEvent = () =>
                {
                    if (this.transform.localScale.magnitude < 0.1f)
                    {
                        State = SpikeState.IDLE;
                        Destroy(this.gameObject);
                    }

                    this.transform.localScale -= scale * SPEED;
                };
            }
            else if (state == SpikeState.EXTENDING)
            {
                actionEvent = () =>
                {
                    if (this.transform.localScale.magnitude > scale.magnitude)
                    {
                        State = SpikeState.IDLE;
                    }

                    this.transform.localScale += scale * SPEED;
                };
            }
        }
    }
    private System.Action actionEvent { get; set; }
    public Vector3 scale { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        scale = this.transform.localScale;
        this.transform.localScale = Vector3.zero;
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

    public enum SpikeState
    {
        IDLE,
        HIDING,
        EXTENDING
    }
}
