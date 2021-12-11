using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossBattleSystem : MonoBehaviour
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
    public AudioClip playerHealSE;
    public AudioClip criticalHitSE;
    public AudioClip enemyAttack;

    public PlayerController player;


    public bool isEnemyTurn;

    public Rigidbody2D enemyRB;

    public GameObject playerClone;
    public GameObject enemyClone;

    Vector3 playerLocation;
    Vector3 enemyLocation;

    bool isSetUp;

    Bullet bullet;

    GameObject dialogueHolder;
    DialogueUI dialogueUI;

    JackyllHandAttack attack1;

    public Animator jackyllAnim;

    public enemyShooting jackyllShoot;
    public GameObject jackyllFireHand;
    public changeSprite handChange;
    public floatUpAndDown hand;
    public floatUpAndDown hand2;

    public static GameObject mainCamera;
    public static GameObject playerCharacter;
    public static GameObject eventSystem;

    [SerializeField] private DialogueObject below75;
    [SerializeField] private DialogueObject below50;
    [SerializeField] private DialogueObject below25;
    [SerializeField] private DialogueObject noHealth;
    bool below75Done = false;
    bool below50Done = false;
    bool below25Done = false;
    bool noHealthDone = false;


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
        if (enemyUnit.currentHP <= 75 && below75Done == false && state == BattleState.PLAYERTURN)
        {
            player.DialogueUI.ShowDialogue(below75);
            below75Done = true;
        }
        if (enemyUnit.currentHP <= 50 && below50Done == false && state == BattleState.PLAYERTURN)
        {
            player.DialogueUI.ShowDialogue(below50);
            below50Done = true;
        }
        if (enemyUnit.currentHP <= 25 && below25Done == false && state == BattleState.PLAYERTURN)
        {
            player.DialogueUI.ShowDialogue(below25);
            below25Done = true;
        }
        if (enemyUnit.currentHP <= 0 && noHealthDone == false && state == BattleState.WON)
        {
            player.DialogueUI.ShowDialogue(noHealth);
            noHealthDone = true;
        }
        if(playerUnit.currentHP <= 0)
        {
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
    }
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        playerCharacter = GameObject.FindGameObjectWithTag("Player");
        eventSystem = GameObject.Find("EventSystem");
        dialogueHolder = GameObject.Find("Canvas");
        dialogueUI = dialogueHolder.GetComponent<DialogueUI>();
        isSetUp = false;
        state = BattleState.START;
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerHealth = 20;

        originalCamera = PlayerController.mainCamera;
        originalCharacter = PlayerController.playerCharacter;
        originalEventSystem = PlayerController.eventSystem;

        StartCoroutine(SetupBattle());
        anim = GameObject.FindWithTag("CombatLeaf").GetComponent<Animator>(); //this fixes the combat animation
        attack1 = GameObject.Find("JackyllHandAttack").GetComponent<JackyllHandAttack>();
        jackyllShoot = GameObject.Find("shoot").GetComponent<enemyShooting>();

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

        jackyllAnim = enemyGO.GetComponent<Animator>();

        playerClone = playerGO;
        enemyClone = enemyGO;

        jackyllFireHand = GameObject.Find("JackyllHandAttack");
        hand = GameObject.Find("JackyllHandAttack").GetComponent<floatUpAndDown>();
        hand2 = GameObject.Find("JackyllHand").GetComponent<floatUpAndDown>();
        handChange = GameObject.Find("JackyllHandAttack").GetComponent<changeSprite>();

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
        
        PlayerController.playerCharacter.SetActive(false);
        state = BattleState.PLAYERTURN;
        yield return new WaitForSeconds(1f);
        jackyllAnim.SetBool("introBoing", true);
        PlayerTurn();
        isSetUp = true;

    }

    IEnumerator PlayerAttack()
    {
        while (dialogueUI.IsOpen)
        {
            yield return null;
        }
        GameObject.Find("AttackButton").GetComponent<Button>().interactable = true;
        GameObject.Find("HealButton").GetComponent<Button>().interactable = true;
        int damage = getDamage();
        enemyHUD.damageText.text = "-" + damage.ToString();
        bool isDead = enemyUnit.TakeDamage(damage);
        dialogueText.text = "The attack hit for " + damage + " damage!";

        if (isDead == true)
        {
            hand.stopMoving = true;
            hand2.stopMoving = true;
            hand.transform.position = new Vector2(1.25f, -.35f);
            hand2.transform.position = new Vector2(4.5f, -.35f);
            enemyUnit.currentHP = 0;
            
            GameObject.Find("AttackButton").GetComponent<Button>().interactable = false;
            GameObject.Find("HealButton").GetComponent<Button>().interactable = false;

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
            Destroy(GameObject.Find("BossTransition"));
            dialogueText.text = "The battle was won! +10Exp!";
            yield return new WaitForSeconds(1f);
            //GameObject leafUnitHealth = GameObject.Find("LeafUnit");
            //GameUnit = leafUnitHealth.GetComponent<Unit>();
            //GameUnit.currentHP = playerUnit.getHP();


            //int oldMax = staticHealth.maxHealth;

            levelUp();

            //staticHealth = GameObject.Find("GameManager").GetComponent<ScriptManager>();
            staticHealth.health = playerUnit.currentHP;
            //staticHealth.health += (staticHealth.maxHealth - oldMax);
            //staticHealth.maxHealth = GameUnit.maxHP;

            int i = 1;
            while (i == 1)
            {
                yield return null;
                if (!dialogueUI.IsOpen)
                {
                    Destroy(originalCamera);
                    Destroy(playerCharacter);
                    SceneManager.LoadSceneAsync("EndGame");
                    SceneManager.UnloadSceneAsync("BossBattle");
                    originalCamera.SetActive(true);
                    originalCharacter.SetActive(true);
                    originalEventSystem.SetActive(true);
                    i = 2;
                }
            }
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "The battle was lost...";
            yield return new WaitForSeconds(1f);
            originalCamera.SetActive(false);
            SceneManager.UnloadSceneAsync("BossBattle");
            player.resetBattle();
        }
    }

    IEnumerator EnemyTurn()
    {
        while (dialogueUI.IsOpen)
        {
            yield return null;
        }
        isEnemyTurn = true;
        bool isDead = playerUnit.TakeDamage(0);
        int attackNum = Random.Range(0, 3);
        print(attackNum);
        if(attackNum == 1)
        {
            StartCoroutine(attack1.handAttack());
            yield return new WaitForSeconds(4f);
        }
        if (attackNum == 0)
        {
            handChange.change = true;
            hand.stopMoving = true;
            Vector2 down = new Vector2(.95f, -.35f);
            Vector2 up = new Vector2(.95f, 1.4f);
            jackyllFireHand.transform.position = down;
            yield return new WaitForSeconds(1f);
            jackyllShoot.Shoot();
            jackyllFireHand.transform.position = up;
            yield return new WaitForSeconds(1f);
            jackyllShoot.Shoot();
            jackyllFireHand.transform.position = down;
            yield return new WaitForSeconds(1f);
            jackyllShoot.Shoot();
            yield return new WaitForSeconds(1f);
            handChange.change = false;
            hand.stopMoving = false;
        }
        if (attackNum == 2)
        {
            handChange.change = true;
            hand.stopMoving = true;
            Vector2 down = new Vector2(.95f, -.35f);
            Vector2 up = new Vector2(.95f, 1.4f);
            jackyllFireHand.transform.position = down;
            yield return new WaitForSeconds(1f);
            jackyllShoot.Shoot();
            jackyllFireHand.transform.position = down;
            yield return new WaitForSeconds(1.5f);
            jackyllShoot.Shoot();
            jackyllFireHand.transform.position = down;
            yield return new WaitForSeconds(1.5f);
            jackyllShoot.Shoot();
            yield return new WaitForSeconds(1f);
            handChange.change = false;
            hand.stopMoving = false;
        }
        if (playerUnit.currentHP <= 0)
        {
            isDead = true;
        }
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
        damage = Random.Range(5, 10);
        // For player damage
        if (state == BattleState.PLAYERTURN)
        {
            damage = Random.Range(4 + damagePlus, 8 + damagePlus) * multiplier;
        }
        return damage;
    }
}
