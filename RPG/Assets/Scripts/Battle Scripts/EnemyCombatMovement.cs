using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatMovement : MonoBehaviour
{
    public Rigidbody2D body;
    public Vector2 originalPosition;

    public BattleSystem battle;
    BattleWaveSystem battleWave;
    battleTutorial tutorial;

    public Vector2 playerLocation;
    BoxCollider2D box;
    BoxCollider2D player;
    Animator anim;
    Animator playerAnim;

    [SerializeField] bool isWave = false;

    DialogueActivatorCutscene cutscene;

    BattleHUD playerDamage;

    public bool isTutorial = false;

    public AudioClip screetch;

    private Vector3 myVec = new Vector3(0f, 180f, 0f);

    private Vector2 original;

    public bool sike = false;

    // Start is called before the first frame update
    void Start()
    {
        sike = false;
        playerAnim = GameObject.FindWithTag("CombatLeaf").GetComponent<Animator>();
        playerDamage = GameObject.Find("PlayerBattleHud").GetComponent<BattleHUD>();
        cutscene = GameObject.Find("waveCutscene").GetComponent<DialogueActivatorCutscene>();
        box = GameObject.Find("CombatEnemy(Clone)").GetComponent<BoxCollider2D>();
        player = GameObject.Find("LeafCombat(Clone)").GetComponent<BoxCollider2D>();
        body = this.GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(box, player);
        anim = this.GetComponent<Animator>();
        body = gameObject.GetComponent<Rigidbody2D>();

        original = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

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
        else if(isTutorial == false)
        {
            battle = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
        }
        else
        {
            tutorial = GameObject.Find("BattleSystemTutorial").GetComponent<battleTutorial>();
        }

    }
    public IEnumerator MoveTowards()
    {
        int ran = Random.Range(0, 2);
        if(ran == 0)
        {
            anim.SetBool("carBait", true);
            yield return new WaitForSeconds(.5f);
            anim.SetBool("carBait", false);
            sike = true;
        }
        anim.SetBool("carMoving", true);
        body.AddForce(new Vector2(-450, 0));
        yield return null;
    }
    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "goBack")
        {
            gameObject.transform.Rotate(myVec);
            body.AddForce(new Vector2(300, 0));
            yield return new WaitForSeconds(.1f);
            body.AddForce(new Vector2(600, 0));
        }
        if (other.tag == "stop")
        {
            anim.SetBool("carMoving", false);
            gameObject.transform.Rotate(myVec);
            body.AddForce(new Vector2(-450, 0));
            gameObject.transform.position = original;
        }
        if(other.tag == "sike")
        {
            if (sike)
            {
                print("gotem");
                anim.SetBool("carMoving", false);
                body.AddForce(new Vector2(450, 0));
                sike = false;
                yield return new WaitForSeconds(.5f);
                anim.SetBool("carMoving", true);
                body.AddForce(new Vector2(-450, 0));
            }
        }
        if (other.gameObject.name == "LeafCombat(Clone)" && isWave == false && tutorial == null)
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
        if (other.gameObject.name == "LeafCombat(Clone)" && isWave && tutorial == null)
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
        if (other.gameObject.name == "LeafCombat(Clone)" && isWave == false && tutorial != null)
        {
            playerAnim.SetBool("isHit", true);
            int damage = tutorial.getDamage();
            playerDamage.damageText.text = "-" + damage.ToString();
            tutorial.playerUnit.TakeDamage(damage);
            tutorial.playerHUD.SetHUD(tutorial.playerUnit);

            tutorial.playerHUD.SetHP(tutorial.playerUnit.currentHP, tutorial.playerUnit);
            tutorial.dialogueText.text = tutorial.enemyUnit.unitName + " attacks for " + damage + " damage!";
            AudioSource.PlayClipAtPoint(tutorial.enemyAttack, transform.position);

            yield return new WaitForSeconds(1f);
            playerAnim.SetBool("isHit", false);
            playerDamage.damageText.text = "";
        }
    }
}
