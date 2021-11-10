using UnityEngine;

public class DialogueActivatorAuto : MonoBehaviour, IInteractable
{
    PlayerController player;

    [SerializeField] private DialogueObject dialogueObject;

    bool isDone = false;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && isDone == false)
        {
            player.DialogueUI.ShowDialogue(dialogueObject);
            isDone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        player.Interactiable = null;
    }

    public void Interact(PlayerController player)
    {
        player.DialogueUI.ShowDialogue(dialogueObject);
    }
}

