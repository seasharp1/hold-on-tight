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

    DialogueActivatorCutscene cutscene;
    // Start is called before the first frame update
    void Start()
    {
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "CombatLeaf" && isWave == false)
        {
            print("Hit leaf");
            battle.enemyUnit.damage = battle.getDamage();
            battle.dialogueText.text = battle.enemyUnit.unitName + " attacks for " + battle.enemyUnit.damage + " damage!";
            AudioSource.PlayClipAtPoint(battle.enemyAttack, transform.position);

            battle.playerUnit.TakeDamage(battle.enemyUnit.damage);
            battle.playerHUD.SetHUD(battle.playerUnit);

            battle.playerHUD.SetHP(battle.playerUnit.currentHP, battle.playerUnit);
        }
        if (other.tag == "CombatLeaf" && isWave)
        {
            print("Hit leaf");
            battleWave.enemyUnit.damage = battleWave.getDamage();
            battleWave.dialogueText.text = battleWave.enemyUnit.unitName + " attacks for " + battleWave.enemyUnit.damage + " damage!";
            AudioSource.PlayClipAtPoint(battleWave.enemyAttack, transform.position);

            battleWave.playerUnit.TakeDamage(battleWave.enemyUnit.damage);
            battleWave.playerHUD.SetHUD(battleWave.playerUnit);

            battleWave.playerHUD.SetHP(battleWave.playerUnit.currentHP, battleWave.playerUnit);
        }
    }
}

