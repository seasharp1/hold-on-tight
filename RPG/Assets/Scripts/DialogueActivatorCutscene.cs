using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DialogueActivatorCutscene : MonoBehaviour, IInteractable
{
    PlayerController player;

    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private DialogueObject dialogueObject1;
    [SerializeField] private DialogueObject dialogueObject2;
    [SerializeField] private DialogueObject dialogueObject3;


    public Rigidbody2D jackyllRB;
    public Animator jackyll;

    bool allDone = false;
    bool jackyllGravity = false;

    GameObject[] enemies;
    Rigidbody2D[] enemiesRB;

    bool loadOnce = false;

    public bool wave = false;
    private void Start()
    {
        dialogueObject.isDone = false;
        dialogueObject1.isDone = false;
        dialogueObject2.isDone = false;
        dialogueObject3.isDone = false;
        wave = false;
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        jackyllRB.GetComponent<Rigidbody2D>();
        enemies = GameObject.FindGameObjectsWithTag("waveEnemies");
        enemiesRB = new Rigidbody2D[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            GameObject enemy = enemies[i];
            enemiesRB[i] = enemy.GetComponent<Rigidbody2D>();
        }
    }

    private void Update()
    {
        //print("0:" + dialogueObject.isDone);
        //print("1:" + dialogueObject1.isDone);
        //print("2:" + dialogueObject2.isDone);
        //print("IsOpen:" + player.DialogueUI.IsOpen);
        if(player.DialogueUI.IsOpen == false && dialogueObject3.isDone && loadOnce == false)
        {
            loadOnce = true;
            for(int i = 0; i < enemies.Length; i++)
            {
                Destroy(enemies[i]);
            }
            wave = true;
            SceneManager.LoadScene("Battle(wave)", LoadSceneMode.Additive);
            PlayerController.mainCamera.SetActive(false);
            //PlayerController.playerCharacter.SetActive(false);
            PlayerController.eventSystem.SetActive(false);
            loadOnce = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && dialogueObject.isDone == false)
        {
            player.cantMove = true;
            StartCoroutine(nextDialogueNoWait(dialogueObject));
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
    IEnumerator nextDialogue(DialogueObject d, float timer)
    {
        while(player.DialogueUI.IsOpen == true)
        {
            yield return null;
        }
        if (jackyllGravity)
        {
            jackyllRB.gravityScale = 1;
            yield return new WaitForSeconds(1f);
            jackyll.SetBool("isBoing", true);
            jackyllGravity = false;
        }
        yield return new WaitForSeconds(timer);
        player.DialogueUI.ShowDialogue(d);
        d.isDone = true;
        if (dialogueObject3.isDone)
        {
            for(int i = 0; i <enemiesRB.Length; i++)
            {
                enemiesRB[i].velocity = new Vector2(-12, enemiesRB[i].velocity.y);
            }
        }
        if (allDone)
        {
            jackyllRB.velocity = new Vector2(jackyllRB.velocity.x, 30);
            yield return new WaitForSeconds(.4f);
            jackyllRB.gravityScale = 0;
            jackyllRB.velocity = new Vector2(jackyllRB.velocity.x, 0);
            player.cantMove = false;
        }
    }
    IEnumerator nextDialogueNoWait(DialogueObject d)
    {
        player.DialogueUI.ShowDialogue(d);
        d.isDone = true;
        jackyllGravity = true;
        yield return nextDialogue(dialogueObject1, 2.5f);
        yield return nextDialogue(dialogueObject2, 1.5f);
        allDone = true;
        yield return nextDialogue(dialogueObject3, 1f);
    }
}
