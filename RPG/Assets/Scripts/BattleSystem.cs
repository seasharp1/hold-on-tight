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

    public Unit playerUnit;
    public Unit enemyUnit;

    public Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public GameObject originalCamera;
    public GameObject originalCharacter;
    public GameObject originalEventSystem;

    public int playerHealth;
    public ScriptManager staticHealth;
    public int addExp;

    public BattleState state;

    public Animator anim;

    Unit GameUnit;  //holds player health

    PlayerAttack firstStrikeCheck;

    public AudioClip playerAttackSE;
    public AudioClip playerHealSE;
    public AudioClip enemyAttack;

    public PlayerController player;

    public GameObject enemy1;
    public GameObject enemy2;

    public bool isEnemyTurn;

    public Rigidbody2D enemyRB;

    public GameObject playerClone;
    public GameObject enemyClone;

    public EnemyCombatMovement enemyMove;
    public enemyShooting enemyShoot;

    Vector3 playerLocation;
    Vector3 enemyLocation;

    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && enemyUnit.currentHP > 0)
        {
            GameObject.Find("AttackButton").GetComponent<Button>().onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.E) && enemyUnit.currentHP > 0)
        {
            GameObject.Find("HealButton").GetComponent<Button>().onClick.Invoke();
        }
    }
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if(player.isToyCar == true)
        {
            enemyPrefab = enemy1;
            //enemyMove = enemy1.GetComponent<EnemyCombatMovement>();
            //enemyPatrol = GameObject.FindWithTag("enemyPatrolTag").GetComponent<Patrol>();
            player.isToyCar = false;
        }
        if (player.isToySoldier == true)
        {
            enemyPrefab = enemy2;
            //enemyMove = enemy2.GetComponent<EnemyCombatMovement>();
            enemyRB = enemy2.GetComponent<Rigidbody2D>();
            //enemyPatrol = GameObject.Find("CombatSoldier").GetComponent<Patrol>();
            player.isToySoldier = false;
        }
        playerHealth = originalCharacter.GetComponent<PlayerController>().health;

        originalCamera = PlayerController.mainCamera;
        originalCharacter = PlayerController.playerCharacter;
        originalEventSystem = PlayerController.eventSystem;

        state = BattleState.START;
        StartCoroutine(SetupBattle());
        anim = GameObject.FindWithTag("CombatLeaf").GetComponent<Animator>(); //this fixes the combat animation
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();
        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();
        enemyRB = enemyGO.GetComponent<Rigidbody2D>();

        enemyMove = enemyGO.GetComponent<EnemyCombatMovement>();
        enemyShoot = enemyGO.GetComponent<enemyShooting>();

        playerClone = playerGO;
        enemyClone = enemyGO;

        playerLocation = playerGO.transform.position;
        enemyLocation = enemyGO.transform.position;

        dialogueText.text = "A wild " + enemyUnit.unitName + " approaches...";

        staticHealth = GameObject.Find("GameManager").GetComponent<ScriptManager>();
        playerUnit.currentHP = staticHealth.health;
        playerUnit.maxHP = staticHealth.maxHealth;

        playerHUD.SetHUD_Start(playerUnit, true);
        enemyHUD.SetHUD_Start(enemyUnit, false);

        playerHUD.SetHP_Start(staticHealth.health, playerUnit, true);
        //enemyHUD.SetHP(staticHealth.health, enemyUnit, false);
        playerUnit.currentHP = staticHealth.health;

        //yield return new WaitForSeconds(1f);

        state = BattleState.PLAYERTURN;
        GameObject tempLeaf = GameObject.Find("Leaf"); //added this
        if (tempLeaf != null)
        {
            firstStrikeCheck = tempLeaf.GetComponent<PlayerAttack>();
            if (firstStrikeCheck.firstStrike == true)
            {
                PlayerController.playerCharacter.SetActive(false);

                GameObject.Find("AttackButton").GetComponent<Button>().interactable = false;
                GameObject.Find("HealButton").GetComponent<Button>().interactable = false;

                yield return new WaitForSeconds(.01f);

                anim.SetBool("CombatSwing", true);
                //AudioSource.PlayClipAtPoint(playerAttackSE, transform.position);

                int displayDamage = 8 + staticHealth.extraDamage;

                dialogueText.text = "First Strike for " + displayDamage + " damage";
                bool isDead = enemyUnit.TakeDamage(displayDamage);
                enemyHUD.SetHP(enemyUnit.currentHP, enemyUnit);
                enemyHUD.SetHUD(enemyUnit);
                if (isDead == true)
                {
                    enemyClone.transform.localRotation = Quaternion.Euler(180, 0, 0);
                    enemyUnit.currentHP = 0;
                    GameObject.Find("AttackButton").GetComponent<Button>().interactable = false;
                    GameObject.Find("HealButton").GetComponent<Button>().interactable = false;

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
                    state = BattleState.PLAYERTURN;
                }

                GameObject.Find("AttackButton").GetComponent<Button>().interactable = true;
                GameObject.Find("HealButton").GetComponent<Button>().interactable = true;
                firstStrikeCheck.firstStrike = false;
            }

        }
        else
        {
            PlayerController.playerCharacter.SetActive(false);
            yield return new WaitForSeconds(1f);
        }
        PlayerController.playerCharacter.SetActive(false);
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        playerUnit.damage = getDamage();
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        dialogueText.text = "The attack hit for " + playerUnit.damage + " damage!";

        if (isDead == true)
        {
            enemyClone.transform.localRotation = Quaternion.Euler(180, 0, 0);
            enemyUnit.currentHP = 0;
            GameObject.Find("AttackButton").GetComponent<Button>().interactable = false;
            GameObject.Find("HealButton").GetComponent<Button>().interactable = false;

            enemyHUD.SetHP(enemyUnit.currentHP, enemyUnit);
            enemyHUD.SetHUD(enemyUnit);
            yield return new WaitForSeconds(1f);
            dialogueText.text = enemyUnit.unitName + " was defeated!";
            Destroy(enemyClone);
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

            //int oldMax = staticHealth.maxHealth;

            levelUp();

            staticHealth = GameObject.Find("GameManager").GetComponent<ScriptManager>();
            staticHealth.health = GameUnit.currentHP;
            //staticHealth.health += (staticHealth.maxHealth - oldMax);
            //staticHealth.maxHealth = GameUnit.maxHP;

            SceneManager.UnloadSceneAsync("Battle");
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "The battle was lost...";
            originalCamera.SetActive(false);
            SceneManager.UnloadSceneAsync("Battle");
            SceneManager.LoadScene("BattleLost");
        }
    }

    IEnumerator EnemyTurn()
    {
        isEnemyTurn = true;
        if(enemyMove != null)
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(enemyMove.MoveTowards());
            yield return new WaitForSeconds(3f);
        }
        if(enemyShoot != null)
        {
            yield return new WaitForSeconds(1f);
            enemyShoot.Shoot();
            yield return new WaitForSeconds(1.5f);
        }
        bool isDead = playerUnit.TakeDamage(0);
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
        playerClone.transform.position = playerLocation;
        enemyClone.transform.position = enemyLocation;
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose an action:";
    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(5);

        AudioSource.PlayClipAtPoint(playerHealSE, transform.position);

        playerHUD.SetHP(playerUnit.currentHP, playerUnit);
        playerHUD.SetHUD(playerUnit);
        if (playerUnit.currentHP == playerUnit.maxHP)
        {
            dialogueText.text = "You are fully healed!";
        }
        else
        {
            dialogueText.text = "You healed for 5 HP!";
        }
        playerHUD.SetHP(playerUnit.currentHP, playerUnit);
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
        yield return new WaitForSeconds(2f);
    }

    public void OnAttackButton()
    {
        if (state == BattleState.PLAYERTURN)
        {
            StartCoroutine(PlayerAttack());
        }
        else
        {
            return;
        }
    }

    public void OnHealButton()
    {
        if (state == BattleState.PLAYERTURN)
        {
            StartCoroutine(PlayerHeal());
        }
        else
        {
            return;
        }
    }

    public void swingAnim()
    {
        if (state == BattleState.PLAYERTURN)
        {
            anim.SetBool("CombatSwing", true);
            AudioSource.PlayClipAtPoint(playerAttackSE, transform.position);
        }
        else
        {
            return;
        }
    }

    // Used to gain exp at end of combat
    public void levelUp()
    {
        addExp = 10;
        originalCharacter.GetComponent<LevelUpSystem>().currExp += addExp;
    }

    // Returns random number within specified range for damage and allows for critical hits
    public int getDamage()
    {
        int damage = 0;
        int multiplier = 1;
        int critChance = Random.Range(1, 101);
        int damagePlus = staticHealth.extraDamage;

        // Set crit chance here...
        if (critChance >= 97)
        {
            multiplier = 2;
            Debug.Log("Critical Hit!");
        }

        // For enemy damage
        if (enemyUnit.unitName == "Toy Car")
        {
            damage = Random.Range(1, 6);
        } else if (enemyUnit.unitName == "Toy Soldier") // For use when implemented...
        {
            damage = Random.Range(2, 8);
        } // Add more enemies just as done above and specify damage range...

        // For player damage
        if (state == BattleState.PLAYERTURN)
        {
            damage = Random.Range(4 + damagePlus, 8 + damagePlus) * multiplier;
        }
        return damage;
    }
}