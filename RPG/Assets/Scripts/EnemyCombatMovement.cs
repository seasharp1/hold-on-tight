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
    IEnumerator MoveTowards(Vector2 target, float speed)
    {
        print("called");
        body.AddForce(new Vector2(-speed, 0));
        if(body.position.x <= .1)
        {
            print("called again");
            body.AddForce(new Vector2(0, 0));
            body.AddForce(new Vector2(speed, 0));
            if(body.position.x == originalPosition.x)
            {
                print("called one last time");
                yield return null;
            }
        }
        yield return null;
    }
}
