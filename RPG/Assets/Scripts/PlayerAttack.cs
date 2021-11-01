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

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackLocation.position, attackRange, enemies);

        for (int i = 0; i < hitEnemies.Length; ++i)
        {
            Destroy(hitEnemies[i].gameObject);

            SceneManager.LoadScene("Battle", LoadSceneMode.Additive);
            PlayerController.mainCamera.SetActive(false);
            PlayerController.playerCharacter.SetActive(false);
            PlayerController.eventSystem.SetActive(false);
        }

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Attack");
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
