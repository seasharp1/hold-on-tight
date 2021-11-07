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
    public LayerMask canBreak;

    public bool firstStrike = false;

    public AudioClip swordSwing;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }

    void Attack()
    {
        animator.SetBool("Attack", true);
        AudioSource.PlayClipAtPoint(swordSwing, transform.position);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackLocation.position, attackRange, enemies);
        Collider2D[] hitBreak = Physics2D.OverlapCircleAll(attackLocation.position, attackRange, canBreak);

        for (int i = 0; i < hitEnemies.Length; ++i)
        {
            Destroy(hitEnemies[i].gameObject);

            SceneManager.LoadScene("Battle", LoadSceneMode.Additive);
            PlayerController.mainCamera.SetActive(false);
            //PlayerController.playerCharacter.SetActive(false);
            PlayerController.eventSystem.SetActive(false);
        }
        for (int i = 0; i < hitBreak.Length; ++i)
        {
            Destroy(hitBreak[i].gameObject);
        }


        foreach (Collider2D enemy in hitEnemies)
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
