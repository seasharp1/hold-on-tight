using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleWaveSystem : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    public Unit playerUnit;
    public Unit enemyUnit;

    public Text dialogueText;

    public Text rounds;

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

    public AudioClip playerAttackSE;
    public AudioClip playerHealSE;
    public AudioClip criticalHitSE;
    public AudioClip enemyAttack;

    public PlayerController player;

    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;

    public bool isEnemyTurn;

    public Rigidbody2D enemyRB;

    public GameObject playerClone;
    public GameObject enemyClone;

    public EnemyCombatMovement enemyMove;
    public enemyShooting enemyShoot;

    Vector3 playerLocation;
    Vector3 enemyLocation;

    public int round = 0;
    public int totalRounds = 4;

    DialogueActivatorCutscene cutscene;

    bool isSetUp;

    Animator enemyAnim;

    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && enemyUnit.currentHP > 0 && state == BattleState.PLAYERTURN && isSetUp)
        {
            GameObject.Find("AttackButton").GetComponent<Button>().onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.R) && enemyUnit.currentHP > 0 && state == BattleState.PLAYERTURN && isSetUp)
        {
            GameObject.Find("HealButton").GetComponent<Button>().onClick.Invoke();
        }
        if(staticHealth.health <= 0)
        {
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
    }
    void Start()
    {
        isSetUp = false;
        cutscene = GameObject.Find("waveCutscene").GetComponent<DialogueActivatorCutscene>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

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
        round++;
        GameObject.Find("AttackButton").GetComponent<Button>().interactable = false;
        GameObject.Find("HealButton").GetComponent<Button>().interactable = false;
        if (round == 1)
        {
            enemyPrefab = enemy1;
        }
        if (round == 2)
        {
            enemyPrefab = enemy2;
        }
        if (round == 3)
        {
            enemyPrefab = enemy3;
        }
        if (round == 4)
        {
            enemyPrefab = enemy4;
        }
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();
        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();
        enemyRB = enemyGO.GetComponent<Rigidbody2D>();

        enemyAnim = enemyGO.GetComponent<Animator>();

        rounds.text = "Round " + round + "/" + totalRounds;

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

        yield return new WaitForSeconds(1f);

        state = BattleState.PLAYERTURN;
        GameObject tempLeaf = GameObject.Find("Leaf"); //added this

        GameObject.Find("AttackButton").GetComponent<Button>().interactable = true;
        GameObject.Find("HealButton").GetComponent<Button>().interactable = true;

        PlayerController.playerCharacter.SetActive(false);
        PlayerTurn();
        isSetUp = true;
    }

    IEnumerator SetupBattleAgain()
    {
        isSetUp = false;
        GameObject.Find("AttackButton").GetComponent<Button>().interactable = false;
        GameObject.Find("HealButton").GetComponent<Button>().interactable = false;
        round++;
        if (round == 1)
        {
            enemyPrefab = enemy1;
        }
        if (round == 2)
        {
            enemyPrefab = enemy2;
        }
        if (round == 3)
        {
            enemyPrefab = enemy3;
        }
        if (round == 4)
        {
            enemyPrefab = enemy4;
        }
        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();
        enemyRB = enemyGO.GetComponent<Rigidbody2D>();

        rounds.text = "Round " + round + "/" + totalRounds;

        enemyMove = enemyGO.GetComponent<EnemyCombatMovement>();
        enemyShoot = enemyGO.GetComponent<enemyShooting>();

        enemyClone = enemyGO;

        enemyLocation = enemyGO.transform.position;

        enemyAnim = enemyGO.GetComponent<Animator>();

        dialogueText.text = "A wild " + enemyUnit.unitName + " approaches...";

        staticHealth.health = playerUnit.currentHP;

        playerHUD.SetHUD_Start(playerUnit, true);
        enemyHUD.SetHUD_Start(enemyUnit, false);

        playerHUD.SetHP_Start(staticHealth.health, playerUnit, true);
        //enemyHUD.SetHP(staticHealth.health, enemyUnit, false);
        isEnemyTurn = false;

        yield return new WaitForSeconds(1f);

        state = BattleState.PLAYERTURN;
        GameObject tempLeaf = GameObject.Find("Leaf"); //added this

        GameObject.Find("AttackButton").GetComponent<Button>().interactable = true;
        GameObject.Find("HealButton").GetComponent<Button>().interactable = true;

        PlayerController.playerCharacter.SetActive(false);
        PlayerTurn();
        isSetUp = true;
    }

    IEnumerator PlayerAttack()
    {
        int damage = getDamage();
        enemyHUD.damageText.text = "-" + damage.ToString();
        bool isDead = enemyUnit.TakeDamage(damage);
        dialogueText.text = "The attack hit for " + damage + " damage!";

        if (isDead == true)
        {
            enemyRB.gravityScale = 1;
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
            if(round == 4)
            {
                state = BattleState.WON;
                StartCoroutine(EndBattle());
            }
            else
            {
                StartCoroutine(SetupBattleAgain());
            }
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
            dialogueText.text = "The battle was won! +40Exp";
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

            cutscene.wave = false;
            SceneManager.UnloadSceneAsync("Battle(wave)");
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "The battle was lost...";
            originalCamera.SetActive(false);
            SceneManager.UnloadSceneAsync("Battle(wave)");
            SceneManager.LoadScene("BattleLost");
        }
    }

    IEnumerator EnemyTurn()
    {
        isEnemyTurn = true;
        if (enemyMove != null)
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(enemyMove.MoveTowards());
            yield return new WaitForSeconds(3f);
        }
        if (enemyShoot != null)
        {
            int ran = Random.Range(0, 3);
            print(ran);
            if (ran == 0)
            {
                yield return new WaitForSeconds(.5f);
                enemyAnim.SetBool("isShooting", true);
                yield return new WaitForSeconds(.3f);
                enemyShoot.Shoot();
                yield return new WaitForSeconds(2f);
            }
            if (ran == 1)
            {
                yield return new WaitForSeconds(1f);
                enemyAnim.SetBool("isShooting", true);
                yield return new WaitForSeconds(.3f);
                enemyShoot.Shoot();
                yield return new WaitForSeconds(1.5f);
            }
            if (ran == 2)
            {
                enemyAnim.SetBool("isShooting", true);
                yield return new WaitForSeconds(1f);
                enemyAnim.SetBool("isShooting", true);
                yield return new WaitForSeconds(.3f);
                enemyShoot.Shoot();
                yield return new WaitForSeconds(1.5f);
            }
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
        if(enemyPrefab == enemy2 || enemyPrefab == enemy4)
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
        addExp = 10 * totalRounds;
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
            damage = Random.Range(3, 6);
        }
        else if (enemyUnit.unitName == "Toy Soldier") // For use when implemented...
        {
            damage = Random.Range(3, 8);
        } // Add more enemies just as done above and specify damage range...

        // For player damage
        if (state == BattleState.PLAYERTURN)
        {
            damage = Random.Range(4 + damagePlus, 8 + damagePlus) * multiplier;
        }
        return damage;
    }
}
