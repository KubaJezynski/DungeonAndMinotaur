using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;

    public Action<GameObject> dungeonCreatedEvent { private get; set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (dungeonCreatedEvent != null)
        {
            dungeonCreatedEvent.Invoke(player);
            dungeonCreatedEvent = null;
        }
    }
}
