using CustomMath;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float TRANSLATION_SPEED = 5.0f;
    private const float SPRINT = 1.5f;

    public System.Func<bool> sprintEvent { private get; set; }
    private GameObject lastCollidedStair;

    void Awake()
    {
        sprintEvent = () => false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = new Vector3(0, 0, 9.81f);
    }

    // Update is called once per frame
    void Update()
    {
        float sprint = sprintEvent.Invoke() ? SPRINT : 1;

        float translationX = Input.GetAxisRaw("Horizontal");
        float translationY = Input.GetAxisRaw("Vertical");

        if (translationX == 0 && translationY == 0)
        {
            return;
        }

        float translationAngle = MathFunctions.CalculateAngle(Vector2.zero, new Vector2(translationX, translationY));
        Vector3 position = MathFunctions.CalculateNewPosition(this.transform.position, translationAngle, TRANSLATION_SPEED * sprint * Time.deltaTime);

        this.transform.SetPositionAndRotation(position, Quaternion.Euler(0, 0, -translationAngle));
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Stair") && !other.gameObject.Equals(lastCollidedStair))
        {
            lastCollidedStair = other.gameObject;
            this.transform.position = new Vector3(transform.position.x, transform.position.y, other.transform.position.z - other.transform.localScale.z - transform.localScale.z / 2f);
        }
        else
        {
            lastCollidedStair = null;
        }
    }
}
