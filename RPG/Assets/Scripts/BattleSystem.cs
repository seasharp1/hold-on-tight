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

    [SerializeField] Unit GameUnit;  //holds player health

    PlayerAttack firstStrikeCheck;

    public AudioClip playerAttackSE;
    public AudioClip criticalHitSE;
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

    bool isSetUp;

    Bullet bullet;

    GameObject dialogueHolder;
    DialogueUI dialogueUI;

    Animator enemyAnim;
    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && enemyUnit.currentHP > 0 && state == BattleState.PLAYERTURN && isSetUp && dialogueUI.IsOpen == false)
        {
            GameObject.Find("AttackButton").GetComponent<Button>().onClick.Invoke();
            state = BattleState.ENEMYTURN;
        }
        if (Input.GetKeyDown(KeyCode.R) && enemyUnit.currentHP > 0 && state == BattleState.PLAYERTURN && isSetUp && dialogueUI.IsOpen == false)
        {
            GameObject.Find("HealButton").GetComponent<Button>().onClick.Invoke();
            state = BattleState.ENEMYTURN;
        }
    }
    void Start()
    {
        dialogueHolder = GameObject.Find("Canvas");
        dialogueUI = dialogueHolder.GetComponent<DialogueUI>();
        isSetUp = false;
        state = BattleState.START;
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
        playerHealth = 20;

        originalCamera = PlayerController.mainCamera;
        originalCharacter = PlayerController.playerCharacter;
        originalEventSystem = PlayerController.eventSystem;

        StartCoroutine(SetupBattle());
        anim = GameObject.FindWithTag("CombatLeaf").GetComponent<Animator>(); //this fixes the combat animation
    }

    IEnumerator SetupBattle()
    {
        GameObject.Find("AttackButton").GetComponent<Button>().interactable = false;
        GameObject.Find("HealButton").GetComponent<Button>().interactable = false;
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();
        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();
        enemyRB = enemyGO.GetComponent<Rigidbody2D>();

        enemyMove = enemyGO.GetComponent<EnemyCombatMovement>();
        enemyShoot = enemyGO.GetComponent<enemyShooting>();

        enemyAnim = enemyGO.GetComponent<Animator>();

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
                int displayDamage;
                if (firstStrikeCheck.bulletFirstStrike)
                {
                    displayDamage = (8 + staticHealth.extraDamage) / 2;
                }
                else
                {
                    displayDamage = 8 + staticHealth.extraDamage;
                }

                dialogueText.text = "First Strike for " + displayDamage + " damage";
                bool isDead = enemyUnit.TakeDamage(displayDamage);
                enemyHUD.damageText.text = "-" + displayDamage.ToString();
                enemyHUD.SetHP(enemyUnit.currentHP, enemyUnit);
                enemyHUD.SetHUD(enemyUnit);
                if (isDead == true)
                {
                    enemyClone.transform.localRotation = Quaternion.Euler(180, 0, 0);
                    enemyUnit.currentHP = 0;

                    enemyHUD.SetHP(enemyUnit.currentHP, enemyUnit);
                    enemyHUD.SetHUD(enemyUnit);
                    yield return new WaitForSeconds(1f);
                    enemyHUD.damageText.text = "";
                    dialogueText.text = enemyUnit.unitName + " was defeated!";
                    yield return new WaitForSeconds(1f);
                    state = BattleState.WON;
                    StartCoroutine(EndBattle());
                }
                else
                {
                    state = BattleState.PLAYERTURN;
                }

                GameObject.Find("AttackButton").GetComponent<Button>().interactable = true;
                GameObject.Find("HealButton").GetComponent<Button>().interactable = true;
                firstStrikeCheck.bulletFirstStrike = false;
                firstStrikeCheck.firstStrike = false;
                yield return new WaitForSeconds(.5f);
                enemyHUD.damageText.text = "";
            }

        }
        else
        {
            PlayerController.playerCharacter.SetActive(false);
            yield return new WaitForSeconds(1f);
        }
        PlayerController.playerCharacter.SetActive(false);
        state = BattleState.PLAYERTURN;
        PlayerTurn();
        isSetUp = true;
    }

    IEnumerator PlayerAttack()
    {
        GameObject.Find("AttackButton").GetComponent<Button>().interactable = true;
        GameObject.Find("HealButton").GetComponent<Button>().interactable = true;
        int damage = getDamage();
        enemyHUD.damageText.text = "-" + damage.ToString();
        bool isDead = enemyUnit.TakeDamage(damage);
        dialogueText.text = "The attack hit for " + damage + " damage!";

        if (isDead == true)
        {
            enemyClone.transform.localRotation = Quaternion.Euler(180, 0, 0);
            enemyUnit.currentHP = 0;
            GameObject.Find("AttackButton").GetComponent<Button>().interactable = false;
            GameObject.Find("HealButton").GetComponent<Button>().interactable = false;

            enemyHUD.SetHP(enemyUnit.currentHP, enemyUnit);
            enemyHUD.SetHUD(enemyUnit);
            yield return new WaitForSeconds(1f);
            enemyHUD.damageText.text = "";
            dialogueText.text = enemyUnit.unitName + " was defeated!";
            Destroy(enemyClone);
            yield return new WaitForSeconds(1f);
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }

        enemyHUD.SetHP(enemyUnit.currentHP, enemyUnit);
        enemyHUD.SetHUD(enemyUnit);

        //yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(.65f);
        anim.SetBool("CombatSwing", false);
        yield return new WaitForSeconds(.35f);
        enemyHUD.damageText.text = "";
    }

    IEnumerator EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "The battle was won! +10Exp!";
            yield return new WaitForSeconds(1f);
            //GameObject leafUnitHealth = GameObject.Find("LeafUnit");
            //GameUnit = leafUnitHealth.GetComponent<Unit>();
            //GameUnit.currentHP = playerUnit.getHP();

            originalCamera.SetActive(true);
            originalCharacter.SetActive(true);
            originalEventSystem.SetActive(true);

            //int oldMax = staticHealth.maxHealth;

            levelUp();

            staticHealth = GameObject.Find("GameManager").GetComponent<ScriptManager>();
            staticHealth.health = playerUnit.currentHP;
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
            yield return new WaitForSeconds(.5f);
            enemyAnim.SetBool("isShooting", true);
            yield return new WaitForSeconds(.3f);
            enemyShoot.Shoot();
            yield return new WaitForSeconds(2f);
        }
        bool isDead = playerUnit.TakeDamage(0);
        if (isDead)
        {
            yield return new WaitForSeconds(1f);
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
        //playerClone.transform.position = playerLocation;
        if(enemyPrefab == enemy1)
        {
            enemyClone.transform.position = enemyLocation;
        }
        anim.SetBool("CombatSwing", false);
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
        if (critChance >= 97 && state == BattleState.PLAYERTURN)
        {
            multiplier = 2;
            AudioSource.PlayClipAtPoint(criticalHitSE, transform.position);
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