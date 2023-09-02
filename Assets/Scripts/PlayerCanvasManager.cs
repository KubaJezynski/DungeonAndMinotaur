using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject energyBar;

    private GameObject player;
    public GameObject Player { set { player = value; playerManager = player.GetComponent<PlayerManager>(); SetCanvas(); } }
    private PlayerManager playerManager;

    void Awake()
    {
        this.healthBar.GetComponent<Slider>().maxValue = PlayerManager.MAX_HEALTH;
        this.energyBar.GetComponent<Slider>().maxValue = PlayerManager.MAX_ENERGY;
    }

    // Update is called once per frame
    void Update()
    {
        this.healthBar.GetComponent<Slider>().value = playerManager.Health;
        this.energyBar.GetComponent<Slider>().value = playerManager.Energy;
    }

    private void SetCanvas()
    {
        this.healthBar.GetComponent<Slider>().value = playerManager.Health;
        this.energyBar.GetComponent<Slider>().value = playerManager.Energy;
    }
}
