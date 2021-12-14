using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelUpBeGone : MonoBehaviour
{
    PlayerController player;
    public GameObject ui;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.isActiveAndEnabled == false)
        {
            ui.SetActive(false);
        }
        else
        {
            ui.SetActive(true);
        }
    }
}
