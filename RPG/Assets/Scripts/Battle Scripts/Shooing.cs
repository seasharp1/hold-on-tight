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

    public AudioClip gunshot;

    Animator anim;

    // Update is called once per frame
    private void Start()
    {
        canShoot = true;
        dialogueHolder = GameObject.Find("Canvas");
        dialogueUI = dialogueHolder.GetComponent<DialogueUI>();
        anim = GameObject.FindWithTag("Player").GetComponent<Animator>();
    }
    void Update()
    {
        if(Time.time > nextFireTime)
        {
            if (Input.GetKeyDown(KeyCode.R) && dialogueUI.IsOpen == false && canShoot)
            {
                Shoot();
                anim.SetBool("isShooting", true);
                AudioSource.PlayClipAtPoint(gunshot, transform.position);
                nextFireTime = Time.time + cooldownTime;
            }
        }
    }

    void Shoot()
    {
        Instantiate(bullet, firePoint.position, firePoint.rotation);
    }
}
