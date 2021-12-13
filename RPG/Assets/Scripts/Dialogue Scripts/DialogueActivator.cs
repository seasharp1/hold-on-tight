using UnityEngine;
using System.Collections;


public class DialogueActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private DialogueObject dialogueObjectNotDone;
    [SerializeField] private DialogueObject dialogueObjectDone;
    [SerializeField] private DialogueObject dialogueObjectAfter;

    GameObject dialogueHolder;
    DialogueUI dialogueUI;

    public bool doneOnce = false;

    public bool isBall = false;
    public bool isNPC = false;

    public bool isMachine = false;

    public bool allDone = false;

    ballSideQuest ball;

    GameObject crank;

    private void Start()
    {
        crank = GameObject.Find("key_item");
        ball = GameObject.FindWithTag("ballNPC").GetComponent<ballSideQuest>();
        dialogueHolder = GameObject.Find("Canvas");
        dialogueUI = dialogueHolder.GetComponent<DialogueUI>();
    }
    private void Update()
    {
        if(isMachine && GameObject.Find("wireBox") == null)
        {
            Destroy(GameObject.Find("WireDialogue"));
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerController player))
        {
            player.Interactiable = this;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerController player))
        {
            if (player.Interactiable is DialogueActivator dialogueActivator && dialogueActivator == this)
            {
                player.Interactiable = null;
            }
        }
    }

    public void Interact(PlayerController player)
    {
        if(doneOnce == false && dialogueUI.IsOpen == false)
        {
            player.DialogueUI.ShowDialogue(dialogueObject);
            if (isBall == true)
            {
                doneOnce = true;
            }
            if (isNPC)
            {
                doneOnce = true;
                allDone = true;
            }
        }
        if(doneOnce && ball.questComplete == false && dialogueUI.IsOpen == false && isNPC == false)
        {
            player.DialogueUI.ShowDialogue(dialogueObjectNotDone);
        }
        if(doneOnce && ball.questComplete && dialogueUI.IsOpen == false && allDone == false && isNPC == false)
        {
            player.DialogueUI.ShowDialogue(dialogueObjectDone);
            allDone = true;
            StartCoroutine(destroyTheObject(crank));
        }
        if (allDone && dialogueUI.IsOpen == false)
        {
            player.DialogueUI.ShowDialogue(dialogueObjectAfter);
        }
    }
    IEnumerator destroyTheObject(GameObject obj)
    {
        while (dialogueUI.IsOpen)
        {
            yield return null;
        }
        Destroy(obj);
    }
}
