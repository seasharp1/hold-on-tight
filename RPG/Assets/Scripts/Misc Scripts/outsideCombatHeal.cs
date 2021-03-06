using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outsideCombatHeal : MonoBehaviour
{
    ScriptManager scriptManager;
    bool isTriggered = false;
    public AudioClip heal;

    GameObject dialogueHolder;
    DialogueUI dialogueUI;
    private void Start()
    {
        dialogueHolder = GameObject.Find("Canvas");
        dialogueUI = dialogueHolder.GetComponent<DialogueUI>();
        scriptManager = GameObject.FindWithTag("PlayerGameManager").GetComponent<ScriptManager>();
    }
    private void Update()
    {
        if (isTriggered == true && Input.GetKeyDown(KeyCode.E) && dialogueUI.IsOpen == false)
        {
            AudioSource.PlayClipAtPoint(heal, transform.position);
            scriptManager.health = scriptManager.maxHealth;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isTriggered = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isTriggered = false;
        }
    }
}
