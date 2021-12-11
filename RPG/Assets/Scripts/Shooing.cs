using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooing : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bullet;

    GameObject dialogueHolder;
    DialogueUI dialogueUI;

    public float cooldownTime = 1;
    private float nextFireTime;

    public bool canShoot;

    // Update is called once per frame
    private void Start()
    {
        canShoot = true;
        dialogueHolder = GameObject.Find("Canvas");
        dialogueUI = dialogueHolder.GetComponent<DialogueUI>();
    }
    void Update()
    {
        if(Time.time > nextFireTime)
        {
            if (Input.GetKeyDown(KeyCode.R) && dialogueUI.IsOpen == false && canShoot)
            {
                Shoot();
                nextFireTime = Time.time + cooldownTime;
            }
        }
    }

    void Shoot()
    {
        Instantiate(bullet, firePoint.position, firePoint.rotation);
    }
}
