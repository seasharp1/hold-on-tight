using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackyllHandAttack : MonoBehaviour
{
    BossBattleSystem battle;
    Animator playerAnim;
    BattleHUD playerDamage;
    // Start is called before the first frame update
    void Start()
    {
        battle = GameObject.Find("BossBattleSystem").GetComponent<BossBattleSystem>();
        playerAnim = GameObject.FindWithTag("CombatLeaf").GetComponent<Animator>();
        playerDamage = GameObject.Find("PlayerBattleHud").GetComponent<BattleHUD>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator handAttack()
    {
        bool goingToPlayer = true;
        while (Vector2.Distance(battle.playerBattleStation.transform.position, battle.enemyClone.transform.position) > 2.5 && goingToPlayer == true)
        {
            battle.enemyRB.AddForce(new Vector2(-20, 0));
            yield return null;
        }
        Vector3 myVec = new Vector3(0f, 180f, 0f);
        gameObject.transform.Rotate(myVec);
        while (Vector2.Distance(battle.enemyClone.transform.position, battle.enemyBattleStation.transform.position) > 1.5)
        {
            goingToPlayer = false;
            battle.enemyRB.AddForce(new Vector2(40, 0));
            if (Vector2.Distance(battle.enemyClone.transform.position, battle.enemyBattleStation.transform.position) <= 1.6)
            {
                battle.enemyRB.AddForce(new Vector2(0, 0));
                battle.isEnemyTurn = false;
            }
            yield return null;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "LeafCombat(Clone)")
        {
            playerAnim.SetBool("isHit", true);
            int damage = battle.getDamage();
            playerDamage.damageText.text = "-" + damage.ToString();
            battle.playerUnit.TakeDamage(damage);
            battle.playerHUD.SetHUD(battle.playerUnit);

            battle.playerHUD.SetHP(battle.playerUnit.currentHP, battle.playerUnit);
            battle.dialogueText.text = battle.enemyUnit.unitName + " attacks for " + damage + " damage!";
            AudioSource.PlayClipAtPoint(battle.enemyAttack, transform.position);
        }
    }
}
