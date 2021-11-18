using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;
    BattleSystem battle;
    GameObject[] bullets;
    // Start is called before the first frame update
    void Start()
    {
        battle = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
        rb.velocity = -transform.right * speed;
    }
    private void Update()
    {
        if(battle.state == BattleState.PLAYERTURN)
        {
            bullets = GameObject.FindGameObjectsWithTag("enemyBullet");
            for (int i = 0; i < bullets.Length; i++)
            {
                Destroy(bullets[i]);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "CombatLeaf")
        {
            print("Hit leaf");
            battle.enemyUnit.damage = battle.getDamage();
            battle.dialogueText.text = battle.enemyUnit.unitName + " attacks for " + battle.enemyUnit.damage + " damage!";
            AudioSource.PlayClipAtPoint(battle.enemyAttack, transform.position);

            battle.playerUnit.TakeDamage(battle.enemyUnit.damage);
            battle.playerHUD.SetHUD(battle.playerUnit);

            battle.playerHUD.SetHP(battle.playerUnit.currentHP, battle.playerUnit);
        }
    }
}

