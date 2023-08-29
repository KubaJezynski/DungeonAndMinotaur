using CustomMath;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float TRANSLATION_SPEED = 5.0f;

    private GameObject lastCollidedStair;

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = new Vector3(0, 0, 9.81f);
    }

    // Update is called once per frame
    void Update()
    {
        float translationX = Input.GetAxis("Horizontal") * TRANSLATION_SPEED * Time.deltaTime;
        float translationY = Input.GetAxis("Vertical") * TRANSLATION_SPEED * Time.deltaTime;

        if (translationX == 0 && translationY == 0)
        {
            return;
        }

        Vector3 position = new Vector3(transform.position.x + translationX, transform.position.y + translationY, transform.position.z);
        float targetAngle = MathFunctions.CalculateAngle(Vector2.zero, new Vector2(translationX, translationY));

        this.transform.SetPositionAndRotation(position, Quaternion.Euler(0, 0, -targetAngle));
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
