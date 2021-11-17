using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D body;
    public Vector2 originalPosition;
    public BattleSystem battle;
    public Vector2 playerLocation;
    BoxCollider2D box;
    BoxCollider2D player;
    // Start is called before the first frame update
    void Start()
    {
        box = GameObject.Find("CombatEnemy(Clone)").GetComponent<BoxCollider2D>();
        player = GameObject.Find("LeafCombat(Clone)").GetComponent<BoxCollider2D>();
        body = this.GetComponent<Rigidbody2D>();
        battle = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
        Physics2D.IgnoreCollision(box, player);

    }
    public IEnumerator MoveTowards()
    {
        bool goingToPlayer = true;
        while (battle.isEnemyTurn && battle.state == BattleState.ENEMYTURN)
        {
            while (Vector2.Distance(battle.playerBattleStation.transform.position, battle.enemyClone.transform.position) > 2.5 && goingToPlayer == true)
            {
                battle.enemyRB.AddForce(new Vector2(-2, 0));
                yield return null;
            }
            while (Vector2.Distance(battle.enemyClone.transform.position, battle.enemyBattleStation.transform.position) > 1.5)
            {
                goingToPlayer = false;
                battle.enemyRB.AddForce(new Vector2(4, 0));
                if (Vector2.Distance(battle.enemyClone.transform.position, battle.enemyBattleStation.transform.position) <= 1.6)
                {
                    battle.enemyRB.AddForce(new Vector2(0, 0));
                    battle.isEnemyTurn = false;
                }
                yield return null;
            }
            yield return null;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "LeafCombat(Clone)")
        {
            battle.enemyUnit.damage = battle.getDamage();
            battle.dialogueText.text = battle.enemyUnit.unitName + " attacks for " + battle.enemyUnit.damage + " damage!";
            AudioSource.PlayClipAtPoint(battle.enemyAttack, transform.position);

            battle.playerUnit.TakeDamage(battle.enemyUnit.damage);
            battle.playerHUD.SetHUD(battle.playerUnit);

            battle.playerHUD.SetHP(battle.playerUnit.currentHP, battle.playerUnit);
        }
    }
}
