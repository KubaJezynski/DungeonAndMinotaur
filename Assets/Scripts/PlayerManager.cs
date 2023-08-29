using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject camera;

    void Awake()
    {
        camera = Instantiate(camera);
        camera.GetComponent<CameraManager>().Player = this.gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
