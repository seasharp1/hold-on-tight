using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public PlayerAttack playerAttack;
    public PlayerController player;

    GameObject[] bullets;

    // Start is called before the first frame update
    void Start()
    {
        playerAttack = GameObject.FindWithTag("Player").GetComponent<PlayerAttack>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        rb.velocity = transform.right * speed;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            playerAttack.bulletFirstStrike = true;
            bullets = GameObject.FindGameObjectsWithTag("bullet");
            for(int i = 0; i < bullets.Length; i++)
            {
                Destroy(bullets[i]);
            }
            Destroy(other.gameObject);
            player.isToyCar = true;
            playerAttack.firstStrike = true;
            SceneManager.LoadScene("Battle", LoadSceneMode.Additive);
            PlayerController.mainCamera.SetActive(false);
            //PlayerController.playerCharacter.SetActive(false);
            PlayerController.eventSystem.SetActive(false);
        }
        if (other.tag == "Toy Soldier Enemy")
        {
            playerAttack.bulletFirstStrike = true;
            bullets = GameObject.FindGameObjectsWithTag("bullet");
            for (int i = 0; i < bullets.Length; i++)
            {
                Destroy(bullets[i]);
            }
            Destroy(other.gameObject);
            playerAttack.firstStrike = true;
            player.isToySoldier = true;
            SceneManager.LoadScene("Battle", LoadSceneMode.Additive);
            PlayerController.mainCamera.SetActive(false);
            //PlayerController.playerCharacter.SetActive(false);
            PlayerController.eventSystem.SetActive(false);
        }
    }
}
