using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyShooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bullet;

    GameObject dialogueHolder;
    DialogueUI dialogueUI;
    BattleSystem battle;

    // Update is called once per frame
    private void Start()
    {
        dialogueHolder = GameObject.Find("Canvas");
        dialogueUI = dialogueHolder.GetComponent<DialogueUI>();
        battle = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
    }
    public void Shoot()
    {
        Instantiate(bullet, firePoint.position, firePoint.rotation);
    }
}
