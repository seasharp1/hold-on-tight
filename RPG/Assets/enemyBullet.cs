using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;
    BattleSystem battle;
    // Start is called before the first frame update
    void Start()
    {
        battle = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
        rb.velocity = -transform.right * speed;
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

