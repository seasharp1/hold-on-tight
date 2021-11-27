using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D body;
    public Vector2 originalPosition;

    public BattleSystem battle;
    BattleWaveSystem battleWave;

    public Vector2 playerLocation;
    BoxCollider2D box;
    BoxCollider2D player;
    Animator anim;
    Animator playerAnim;

    [SerializeField] bool isWave = false;

    DialogueActivatorCutscene cutscene;

    BattleHUD playerDamage;

    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GameObject.FindWithTag("CombatLeaf").GetComponent<Animator>();
        playerDamage = GameObject.Find("PlayerBattleHud").GetComponent<BattleHUD>();
        cutscene = GameObject.Find("waveCutscene").GetComponent<DialogueActivatorCutscene>();
        box = GameObject.Find("CombatEnemy(Clone)").GetComponent<BoxCollider2D>();
        player = GameObject.Find("LeafCombat(Clone)").GetComponent<BoxCollider2D>();
        body = this.GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(box, player);
        anim = this.GetComponent<Animator>();
        if (cutscene.wave)
        {
            isWave = true;
        }
        else
        {
            isWave = false;
        }
        if (isWave)
        {
            battleWave = GameObject.Find("BattleWaveSystem").GetComponent<BattleWaveSystem>();
        }
        else
        {
            battle = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
        }

    }
    public IEnumerator MoveTowards()
    {
        if (isWave)
        {
            bool goingToPlayer = true;
            while (battleWave.isEnemyTurn && battleWave.state == BattleState.ENEMYTURN)
            {
                while (Vector2.Distance(battleWave.playerBattleStation.transform.position, battleWave.enemyClone.transform.position) > 2.5 && goingToPlayer == true)
                {
                    battleWave.enemyRB.AddForce(new Vector2(-2, 0));
                    anim.SetBool("carMoving", true);
                    yield return null;
                }
                Vector3 myVec = new Vector3(0f, 180f, 0f);
                gameObject.transform.Rotate(myVec);
                while (Vector2.Distance(battleWave.enemyClone.transform.position, battleWave.enemyBattleStation.transform.position) > 1.5)
                {
                    goingToPlayer = false;
                    battleWave.enemyRB.AddForce(new Vector2(4, 0));
                    if (Vector2.Distance(battleWave.enemyClone.transform.position, battleWave.enemyBattleStation.transform.position) <= 1.6)
                    {
                        battleWave.enemyRB.AddForce(new Vector2(0, 0));
                        battleWave.isEnemyTurn = false;
                    }
                    yield return null;
                }
                anim.SetBool("carMoving", false);
                gameObject.transform.Rotate(myVec);
                yield return null;
            }
        }
        else
        {
            bool goingToPlayer = true;
            while (battle.isEnemyTurn && battle.state == BattleState.ENEMYTURN)
            {
                while (Vector2.Distance(battle.playerBattleStation.transform.position, battle.enemyClone.transform.position) > 2.5 && goingToPlayer == true)
                {
                    battle.enemyRB.AddForce(new Vector2(-2, 0));
                    anim.SetBool("carMoving", true);
                    yield return null;
                }
                Vector3 myVec = new Vector3(0f, 180f, 0f);
                gameObject.transform.Rotate(myVec);
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
                anim.SetBool("carMoving", false);
                gameObject.transform.Rotate(myVec);
                yield return null;
            }
        }
    }
    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "LeafCombat(Clone)" && isWave == false)
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
            playerAnim.SetBool("isHit", false);
            playerDamage.damageText.text = "";
        }
        if (other.gameObject.name == "LeafCombat(Clone)" && isWave)
        {
            playerAnim.SetBool("isHit", true);
            int damage = battleWave.getDamage();
            playerDamage.damageText.text = "-" + damage.ToString();
            battleWave.playerUnit.TakeDamage(damage);
            battleWave.playerHUD.SetHUD(battleWave.playerUnit);

            battleWave.playerHUD.SetHP(battleWave.playerUnit.currentHP, battleWave.playerUnit);
            battleWave.dialogueText.text = battleWave.enemyUnit.unitName + " attacks for " + damage + " damage!";
            AudioSource.PlayClipAtPoint(battleWave.enemyAttack, transform.position);

            yield return new WaitForSeconds(1f);
            playerAnim.SetBool("isHit", false);
            playerDamage.damageText.text = "";
        }
    }
}
