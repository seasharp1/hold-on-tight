using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit enemyUnit;

    public Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public GameObject originalCamera;
    public GameObject originalCharacter;
    public GameObject originalEventSystem;

    public int playerHealth;
    public ScriptManager staticHealth;

    public BattleState state;

    public Animator anim;

    Unit GameUnit;  //holds player health

    PlayerAttack firstStrikeCheck;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = originalCharacter.GetComponent<PlayerController>().health;

        originalCamera = PlayerController.mainCamera;
        originalCharacter = PlayerController.playerCharacter;
        originalEventSystem = PlayerController.eventSystem;

        state = BattleState.START;
        StartCoroutine(SetupBattle());
        anim = GameObject.FindWithTag("CombatLeaf").GetComponent<Animator>(); //this fixes the combat animation
        //GameObject LeafCombatPrefab = (GameObject)Resources.Load("LeafCombat");
        //GameObject LeafCombatScene = (GameObject)Instantiate(LeafCombatPrefab);
    }

    IEnumerator SetupBattle()
    {

        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();
        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        //playerUnit.currentHP = playerHealth;
        //playerHUD.SetHP(playerHealth);

        staticHealth = GameObject.Find("GameManager").GetComponent<ScriptManager>();
        playerHUD.SetHP(staticHealth.health, playerUnit);
        playerUnit.currentHP = staticHealth.health;

        Debug.Log("Player start health " + staticHealth.health);

        //playerHUD.SetHP(player.health);

        dialogueText.text = "A wild " + enemyUnit.unitName + " approaches...";

        yield return new WaitForSeconds(1f);

        state = BattleState.PLAYERTURN;
        GameObject tempLeaf = GameObject.Find("Leaf"); //added this

        if(tempLeaf != null)
        {
            Debug.Log("Leaf is not null");
            firstStrikeCheck = tempLeaf.GetComponent<PlayerAttack>();
            if (firstStrikeCheck.firstStrike == true)
            {
                firstStrikeCheck = GameObject.Find("Leaf").GetComponent<PlayerAttack>();


                Debug.Log("extra damage!");
                anim.SetBool("CombatSwing", true);
                dialogueText.text = "First Strike for 10 damage";
                bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
                enemyHUD.SetHP(enemyUnit.currentHP, enemyUnit);
                enemyHUD.SetHUD(enemyUnit);
                yield return new WaitForSeconds(1f);
                firstStrikeCheck.firstStrike = false;
            }
        }
        else
        {
            Debug.Log("Leaf is null!");
        }

        
        

        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        dialogueText.text = "The attack hit for " + playerUnit.damage + " damage!";
        if (isDead == true)
        {
            enemyHUD.SetHP(enemyUnit.currentHP, enemyUnit);
            enemyHUD.SetHUD(enemyUnit);
            yield return new WaitForSeconds(1f);
            dialogueText.text = enemyUnit.unitName + " was defeated!";
            yield return new WaitForSeconds(1f);
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }

        enemyHUD.SetHP(enemyUnit.currentHP, enemyUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(1f);
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "The battle was won!";
            GameObject leafUnitHealth = GameObject.Find("LeafUnit");
            GameUnit = leafUnitHealth.GetComponent<Unit>();
            GameUnit.currentHP = playerUnit.getHP();

            originalCamera.SetActive(true);
            originalCharacter.SetActive(true);
            originalEventSystem.SetActive(true);

            staticHealth = GameObject.Find("GameManager").GetComponent<ScriptManager>();
            staticHealth.health = GameUnit.currentHP;

            //playerHealth = GameUnit.currentHP;
            Debug.Log("player health at end of battle is" + GameUnit.currentHP);
            SceneManager.UnloadSceneAsync("Battle");




        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "The battle was lost...";
        }
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);
        dialogueText.text = enemyUnit.unitName + " attacks for " + enemyUnit.damage + " damage!";

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
        playerHUD.SetHUD(playerUnit);

        playerHUD.SetHP(playerUnit.currentHP, playerUnit);
        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            yield return new WaitForSeconds(1f);
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose an action:";
    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(10);

        playerHUD.SetHP(playerUnit.currentHP, playerUnit);
        playerHUD.SetHUD(playerUnit);
        if (playerUnit.currentHP == playerUnit.maxHP)
        {
            dialogueText.text = "You are fully healed!";
        }
        else
        {
            dialogueText.text = "You healed for 10 HP!";
        }
        playerHUD.SetHP(playerUnit.currentHP, playerUnit);
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
        yield return new WaitForSeconds(2f);
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        StartCoroutine(PlayerAttack());
    }
    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        StartCoroutine(PlayerHeal());
    }
    public void swingAnim()
    {
        if (state != BattleState.PLAYERTURN)
        {
            //print("false");
            return;
        }
        //print("working");
        anim.SetBool("CombatSwing", true);
        //print("True");
    }
}