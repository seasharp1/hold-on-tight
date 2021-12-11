using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackyllHandAttack : MonoBehaviour
{
    BossBattleSystem battle;
    Animator playerAnim;
    BattleHUD playerDamage;
    Rigidbody2D handRB;
    floatUpAndDown change;
    Vector2 originalPos;
    // Start is called before the first frame update
    void Start()
    {
        originalPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        battle = GameObject.Find("BossBattleSystem").GetComponent<BossBattleSystem>();
        playerAnim = GameObject.FindWithTag("CombatLeaf").GetComponent<Animator>();
        playerDamage = GameObject.Find("PlayerBattleHud").GetComponent<BattleHUD>();
        handRB = this.GetComponent<Rigidbody2D>();
        change = this.GetComponent<floatUpAndDown>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator handAttack()
    {
        Vector2 temp = new Vector2(.95f, -.35f);
        change.stopMoving = true;
        this.transform.position = temp;
        yield return new WaitForSeconds(1f);
        handRB.AddForce(new Vector2(-500, 0));
    }
    private IEnumerator OnTriggerEnter2D(Collider2D other)
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
            yield return new WaitForSeconds(1f);
            playerDamage.damageText.text = "";
            playerAnim.SetBool("isHit", false);
        }
        if(other.tag == "flip")
        {
            print("flip");
            Vector3 myVec = new Vector3(0f, 180f, 0f);
            gameObject.transform.Rotate(myVec);
            handRB.AddForce(new Vector2(1000, 0));

        }
        if (other.tag == "return")
        {
            print("return");
            handRB.AddForce(new Vector2(0, 0));
            Vector3 myVec = new Vector3(0f, 180f, 0f);
            gameObject.transform.Rotate(myVec);
            change.stopMoving = false;
            handRB.AddForce(new Vector2(-500, 0));

            this.transform.position = originalPos;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
