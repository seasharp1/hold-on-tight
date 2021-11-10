using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooing : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bullet;

    GameObject dialogueHolder;
    DialogueUI dialogueUI;

    // Update is called once per frame
    private void Start()
    {
        dialogueHolder = GameObject.Find("Canvas");
        dialogueUI = dialogueHolder.GetComponent<DialogueUI>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && dialogueUI.IsOpen == false)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bullet, firePoint.position, firePoint.rotation);
    }
}
