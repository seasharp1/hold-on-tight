using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;

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
        player.DialogueUI.ShowDialogue(dialogueObject);
    }
}
