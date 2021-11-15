using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D body;
    public Vector2 originalPosition;
    public BattleSystem battle;
    public Vector2 playerLocation;
    // Start is called before the first frame update
    void Start()
    {
        body = this.GetComponent<Rigidbody2D>();
        battle = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(battle.isEnemyTurn)
        {
            //StartCoroutine(MoveTowards(playerLocation, 4));
        }
    }
    public IEnumerator MoveTowards()
    {
        while (battle.isEnemyTurn)
        {
            while (Vector2.Distance(battle.playerBattleStation.transform.position, battle.enemyClone.transform.position) > 2)
            {
                //print(Vector2.Distance(playerClone.transform.position, enemyClone.transform.position));
                battle.enemyRB.AddForce(new Vector2(-2, 0));
                yield return null;
            }
            print("exit loop");
            while (Vector2.Distance(battle.enemyClone.transform.position, battle.enemyBattleStation.transform.position) > 1.5)
            {
                print("enter loop");
                //print("New:" + Vector2.Distance(enemyClone.transform.position, enemyBattleStation.transform.position));
                battle.enemyRB.AddForce(new Vector2(2, 0));
                print("close to true" + Vector2.Distance(battle.enemyClone.transform.position, battle.enemyBattleStation.transform.position));
                if (Vector2.Distance(battle.enemyClone.transform.position, battle.enemyBattleStation.transform.position) <= 1.6)
                {
                    print("true");
                    battle.enemyRB.AddForce(new Vector2(0, 0));
                    battle.isEnemyTurn = false;
                }
                yield return null;
            }
            yield return null;
        }
    }
}
