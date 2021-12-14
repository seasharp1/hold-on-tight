using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoorDialogue : MonoBehaviour
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private DialogueObject dialogueObjectUnlocked;


    GameObject dialogueHolder;
    DialogueUI dialogueUI;

    PlayerController player;

    DialogueActivator ball;
    private void Start()
    {
        dialogueHolder = GameObject.Find("Canvas");
        dialogueUI = dialogueHolder.GetComponent<DialogueUI>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        ball = GameObject.FindWithTag("ballNPC").GetComponent<DialogueActivator>();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject == GameObject.FindWithTag("Player") && ball.allDone == false)
        {
            player.DialogueUI.ShowDialogue(dialogueObject);
        }
        if (other.gameObject == GameObject.FindWithTag("Player") && ball.allDone)
        {
            player.DialogueUI.ShowDialogue(dialogueObjectUnlocked);
            Destroy(this.gameObject);
        }
    }
}
