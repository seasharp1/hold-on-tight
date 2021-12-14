using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;
    BattleSystem battle;
    BattleWaveSystem battleWave;
    BossBattleSystem bossBattle;
    GameObject[] bullets;

    public bool isWave = false;
    public bool isBoss = false;

    Animator playerAnim;

    BattleHUD playerDamage;

    DialogueActivatorCutscene cutscene;
    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GameObject.FindWithTag("CombatLeaf").GetComponent<Animator>();
        playerDamage = GameObject.Find("PlayerBattleHud").GetComponent<BattleHUD>();
        //enemyDamage = GameObject.Find("EnemyBattleHud").GetComponent<BattleHUD>();
        cutscene = GameObject.Find("waveCutscene").GetComponent<DialogueActivatorCutscene>();
        if (cutscene.wave)
        {
            isWave = true;
        }
        else
        {
            isWave = false;
        }
        if(isWave)
        {
            battleWave = GameObject.Find("BattleWaveSystem").GetComponent<BattleWaveSystem>();
        }
        else if(isWave == false && isBoss == false)
        {
            battle = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
        }
        else if (isBoss && isWave == false)
        {
            bossBattle = GameObject.Find("BossBattleSystem").GetComponent<BossBattleSystem>();
        }
        if (isBoss)
        {

        }
        rb.velocity = -transform.right * speed;
    }
    private void Update()
    {
        if (isWave && isBoss == false)
        {
            if (battleWave.state == BattleState.PLAYERTURN)
            {
                bullets = GameObject.FindGameObjectsWithTag("enemyBullet");
                for (int i = 0; i < bullets.Length; i++)
                {
                    Destroy(bullets[i]);
                }
            }
        }
        else if(isBoss && isWave == false)
        {
            if (bossBattle.state == BattleState.PLAYERTURN)
            {
                bullets = GameObject.FindGameObjectsWithTag("enemyBullet");
                for (int i = 0; i < bullets.Length; i++)
                {
                    Destroy(bullets[i]);
                }
            }
        }
        else if(battle.state == BattleState.PLAYERTURN)
        {
            bullets = GameObject.FindGameObjectsWithTag("enemyBullet");
            for (int i = 0; i < bullets.Length; i++)
            {
                Destroy(bullets[i]);
            }
        }
    }
    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "CombatLeaf" && isWave == false && isBoss == false)
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
        if (other.tag == "CombatLeaf" && isWave && isBoss == false)
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
        if (other.tag == "CombatLeaf" && isWave == false && isBoss)
        {
            playerAnim.SetBool("isHit", true);
            int damage = bossBattle.getDamage();
            playerDamage.damageText.text = "-" + damage.ToString();
            bossBattle.playerUnit.TakeDamage(damage);
            bossBattle.playerHUD.SetHUD(bossBattle.playerUnit);

            bossBattle.playerHUD.SetHP(bossBattle.playerUnit.currentHP, bossBattle.playerUnit);
            bossBattle.dialogueText.text = bossBattle.enemyUnit.unitName + " attacks for " + damage + " damage!";
            AudioSource.PlayClipAtPoint(bossBattle.enemyAttack, transform.position);

            yield return new WaitForSeconds(1f);
            playerAnim.SetBool("isHit", false);

            playerDamage.damageText.text = "";
        }
    }
}

