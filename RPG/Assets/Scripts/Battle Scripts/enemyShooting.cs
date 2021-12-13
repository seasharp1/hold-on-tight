using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyShooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bullet;

    GameObject dialogueHolder;
    DialogueUI dialogueUI;

    public AudioClip gunshot;

    // Update is called once per frame
    private void Start()
    {
        dialogueHolder = GameObject.Find("Canvas");
        dialogueUI = dialogueHolder.GetComponent<DialogueUI>();
    }
    public void Shoot()
    {
        AudioSource.PlayClipAtPoint(gunshot, transform.position);
        Instantiate(bullet, firePoint.position, firePoint.rotation);
    }
}
