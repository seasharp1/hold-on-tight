using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;

    public Transform attackLocation;
    public float attackRange = 0.5f;
    public LayerMask enemies;
    public LayerMask otherEnemies;
    public LayerMask canBreak;
    public LayerMask tutorial;

    public bool firstStrike = false;
    public bool bulletFirstStrike = false;

    public AudioClip swordSwing;
    public AudioClip smash;
    public PlayerController player;

    GameObject dialogueHolder;
    DialogueUI dialogueUI;

    public float cooldownTime = 1;
    private float nextFireTime;

    // Update is called once per frame
    private void Start()
    {
        dialogueHolder = GameObject.Find("Canvas");
        dialogueUI = dialogueHolder.GetComponent<DialogueUI>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }
    void Update()
    {
        if (Time.time > nextFireTime)
        {
            if (Input.GetButtonDown("Fire1") && dialogueUI.IsOpen == false)
            {
                Attack();
                nextFireTime = Time.time + cooldownTime;
            }
        }
    }
    void Attack()
    {
        animator.SetBool("Attack", true);
        AudioSource.PlayClipAtPoint(swordSwing, transform.position);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackLocation.position, attackRange, enemies);
        Collider2D[] hitSoldiers = Physics2D.OverlapCircleAll(attackLocation.position, attackRange, otherEnemies);
        Collider2D[] hitBreak = Physics2D.OverlapCircleAll(attackLocation.position, attackRange, canBreak);
        Collider2D[] hitTutorial = Physics2D.OverlapCircleAll(attackLocation.position, attackRange, tutorial);

        for (int i = 0; i < hitEnemies.Length; ++i)
        {
            Destroy(hitEnemies[i].gameObject);

            SceneManager.LoadScene("Battle", LoadSceneMode.Additive);
            PlayerController.mainCamera.SetActive(false);
            //PlayerController.playerCharacter.SetActive(false);
            PlayerController.eventSystem.SetActive(false);
        }
        for (int i = 0; i < hitSoldiers.Length; ++i)
        {
            Destroy(hitSoldiers[i].gameObject);

            SceneManager.LoadScene("Battle", LoadSceneMode.Additive);
            PlayerController.mainCamera.SetActive(false);
            //PlayerController.playerCharacter.SetActive(false);
            PlayerController.eventSystem.SetActive(false);
        }
        for (int i = 0; i < hitBreak.Length; ++i)
        {
            AudioSource.PlayClipAtPoint(smash, transform.position);
            Destroy(hitBreak[i].gameObject);
        }
        for (int i = 0; i < hitTutorial.Length; ++i)
        {
            Destroy(hitTutorial[i].gameObject);

            SceneManager.LoadScene("Battle(Tutorial)", LoadSceneMode.Additive);
            PlayerController.mainCamera.SetActive(false);
            //PlayerController.playerCharacter.SetActive(false);
            PlayerController.eventSystem.SetActive(false);
        }

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Attack");
            player.isToyCar = true;
            firstStrike = true;
        }
        foreach (Collider2D enemy in hitSoldiers)
        {
            Debug.Log("Attack");
            player.isToySoldier = true;
            firstStrike = true;
        }

        foreach (Collider2D enemy in hitTutorial)
        {
            Debug.Log("Attack");
            firstStrike = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackLocation == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackLocation.position, attackRange);
    }
}
