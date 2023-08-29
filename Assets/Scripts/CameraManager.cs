using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private GameObject player;
    public GameObject Player { set { player = value; SetCamera(); } }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player == null)
        {
            return;
        }

        Track(player);
    }

    private void SetCamera()
    {
        this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 10);
        this.GetComponent<Camera>().orthographic = true;
        this.GetComponent<Camera>().orthographicSize = 20;
    }

    private void Track(GameObject player)
    {
        this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 10);
    }
}
