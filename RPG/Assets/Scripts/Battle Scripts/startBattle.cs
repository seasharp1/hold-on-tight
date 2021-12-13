using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startBattle : MonoBehaviour
{
    PlayerController player;

    GameObject dialogueHolder;
    DialogueUI dialogueUI;

    public DialogueActivatorAuto dial;

    bool doneOnce = false;

    public GameObject destroyAfter;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        dialogueHolder = GameObject.Find("Canvas");
        dialogueUI = dialogueHolder.GetComponent<DialogueUI>();
    }
    public void startTheFight()
    {
        player.isToySoldier = true;
        SceneManager.LoadScene("Battle", LoadSceneMode.Additive);
        PlayerController.mainCamera.SetActive(false);
        PlayerController.eventSystem.SetActive(false);
    }
    private void Update()
    {
        if (dialogueUI.IsOpen == false && doneOnce == false && dial.isDone)
        {
            startTheFight();
            Destroy(destroyAfter);
            doneOnce = true;
        }
    }
}
