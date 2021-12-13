using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueActivatorAuto : MonoBehaviour, IInteractable
{
    PlayerController player;

    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private DialogueObject dialogueObject1;

    Animator anim;

    public bool isDone = false;
    public bool isFight = false;
    public bool isBoss = false;

    GameObject dialogueHolder;
    DialogueUI dialogueUI;

    bool isDone1 = false;
    bool isDone2 = false;

    public GameObject bossStart;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        anim = GameObject.FindWithTag("Player").GetComponent<Animator>();
        dialogueHolder = GameObject.Find("Canvas");
        dialogueUI = dialogueHolder.GetComponent<DialogueUI>();
    }
    private void Update()
    {
        if (isDone && dialogueUI.IsOpen == false && isDone1 == false && isBoss)
        {
            anim.SetBool("isReady", true);
            player.DialogueUI.ShowDialogue(dialogueObject1);
            isDone1 = true;
        }
        if (isDone && dialogueUI.IsOpen == false && isDone1 && isBoss && isDone2 == false)
        {
            isDone2 = true;
            PlayerController.mainCamera.SetActive(false);
            //PlayerController.playerCharacter.SetActive(false);
            PlayerController.eventSystem.SetActive(false);
            anim.SetBool("isReady", false);
            bossStart.SetActive(true);
        }
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

