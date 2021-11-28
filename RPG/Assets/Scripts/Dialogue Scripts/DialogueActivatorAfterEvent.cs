using UnityEngine;

public class DialogueActivatorAfterEvent : MonoBehaviour, IInteractable
{
    PlayerController player;

    [SerializeField] private DialogueObject dialogueObject;

    public GameObject triggerObject;

    [SerializeField] bool wallBroken = false;
    [SerializeField] bool isDone = false;
    [SerializeField] bool playerTouching = false;
    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (triggerObject == null)
        {
            wallBroken = true;
        }
        if (wallBroken && playerTouching && isDone == false)
        {
            player.DialogueUI.ShowDialogue(dialogueObject);
            isDone = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            playerTouching = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerTouching = false;
        }
        player.Interactiable = null;
    }

    public void Interact(PlayerController player)
    {
        player.DialogueUI.ShowDialogue(dialogueObject);
    }
}
