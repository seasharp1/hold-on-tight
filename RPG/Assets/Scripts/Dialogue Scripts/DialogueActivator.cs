using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private DialogueObject dialogueObjectNotDone;
    [SerializeField] private DialogueObject dialogueObjectDone;
    [SerializeField] private DialogueObject dialogueObjectAfter;

    GameObject dialogueHolder;
    DialogueUI dialogueUI;

    public bool doneOnce = false;
    public bool notBall = true;
    public bool allDone = false;

    ballSideQuest ball;

    private void Start()
    {
        ball = GameObject.FindWithTag("ballNPC").GetComponent<ballSideQuest>();
        dialogueHolder = GameObject.Find("Canvas");
        dialogueUI = dialogueHolder.GetComponent<DialogueUI>();
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
            if (notBall == false)
            {
                doneOnce = true;
            }
        }
        if(doneOnce && ball.questComplete == false && dialogueUI.IsOpen == false)
        {
            player.DialogueUI.ShowDialogue(dialogueObjectNotDone);
        }
        if(doneOnce && ball.questComplete && dialogueUI.IsOpen == false && allDone == false)
        {
            player.DialogueUI.ShowDialogue(dialogueObjectDone);
            allDone = true;
        }
        if (allDone && dialogueUI.IsOpen == false)
        {
            player.DialogueUI.ShowDialogue(dialogueObjectAfter);
        }
    }
}
