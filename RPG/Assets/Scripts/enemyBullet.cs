using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;
    BattleSystem battle;
    BattleWaveSystem battleWave;
    GameObject[] bullets;
    public bool isWave = false;

    BattleHUD playerDamage;

    DialogueActivatorCutscene cutscene;
    // Start is called before the first frame update
    void Start()
    {
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
        else
        {
            battle = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
        }
        rb.velocity = -transform.right * speed;
    }
    private void Update()
    {
        if (isWave)
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
        if (other.tag == "CombatLeaf" && isWave == false)
        {
            int damage = battle.getDamage();
            playerDamage.damageText.text = "-" + damage.ToString();
            battle.playerUnit.TakeDamage(damage);
            battle.playerHUD.SetHUD(battle.playerUnit);

            battle.playerHUD.SetHP(battle.playerUnit.currentHP, battle.playerUnit);
            yield return new WaitForSeconds(1f);
            playerDamage.damageText.text = "";
            battle.dialogueText.text = battle.enemyUnit.unitName + " attacks for " + damage + " damage!";
            AudioSource.PlayClipAtPoint(battle.enemyAttack, transform.position);
        }
        if (other.tag == "CombatLeaf" && isWave)
        {
            int damage = battleWave.getDamage();
            playerDamage.damageText.text = "-" + damage.ToString();
            battleWave.playerUnit.TakeDamage(damage);
            battleWave.playerHUD.SetHUD(battleWave.playerUnit);

            battleWave.playerHUD.SetHP(battleWave.playerUnit.currentHP, battleWave.playerUnit);
            yield return new WaitForSeconds(1f);
            playerDamage.damageText.text = "";
            battleWave.dialogueText.text = battleWave.enemyUnit.unitName + " attacks for " + damage + " damage!";
            AudioSource.PlayClipAtPoint(battleWave.enemyAttack, transform.position);
        }
    }
}

