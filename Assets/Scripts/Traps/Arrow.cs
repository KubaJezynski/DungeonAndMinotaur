using UnityEngine;

public class Arrow : MonoBehaviour
{
    private static int SPEED = 25;
    private static int DAMAGE = 20;

    private Vector3 startingPosition;
    private float capsuleColliderHeight;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = this.transform.position;
        capsuleColliderHeight = this.GetComponent<CapsuleCollider>().height;
        Destroy(this.gameObject, 1);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += this.transform.up.normalized * SPEED * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall") && Vector3.Distance(this.transform.position, startingPosition) > capsuleColliderHeight)
        {
            Destroy(this.gameObject);
        }
        else if (other.CompareTag("Character"))
        {
            other.gameObject.GetComponentInParent<PlayerManager>().takeDamageEvent = () => DAMAGE;
            Destroy(this.gameObject);
        }
    }
}
