using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    Vector3 StartPosition;
    public Vector3 CombatLastLocation;
    Rigidbody2D rb;
    public float speed;
    public float jumpForce;
    public bool isGrounded = false;
    public Transform isGroundedChecker;
    public float checkGroundRadius;
    public LayerMask groundLayer;
    public TextMeshProUGUI dialoguePrompt;

    public static GameObject mainCamera;
    public static GameObject playerCharacter;
    public static GameObject eventSystem;
    public static LevelUpSystem levelUpScript;

    public int health;

    public Animator myAnim;

    SpriteRenderer myRender;
    bool isLeft = false;
    bool isRight = true;

    public static PlayerController instance;
    public string areaTransitionName;

    [SerializeField] private DialogueUI dialogueUI;
    public DialogueUI DialogueUI => dialogueUI;

    public IInteractable Interactiable { get; set; }

    public AudioClip coin;
    public AudioClip jumpingSE;

    public bool isToyCar = false;
    public bool isToySoldier = false;

    public bool cantMove = false;

    ScriptManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<ScriptManager>();
        Application.targetFrameRate = 60;
        health = 20;

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        playerCharacter = GameObject.FindGameObjectWithTag("Player");
        eventSystem = GameObject.Find("EventSystem");
        levelUpScript = playerCharacter.GetComponent<LevelUpSystem>();

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); //if there is more than 1 player, it destroys the new one
        }

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(mainCamera);
        //DontDestroyOnLoad(eventSystem);
        rb = GetComponent<Rigidbody2D>();
        StartPosition = transform.position;
        myRender = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //print(Screen.width + " " + Screen.height);
        CheckIfGrounded();
        Move();
        Jump();
        if (Input.GetKeyDown(KeyCode.E) && dialogueUI.IsOpen == false)
        {
            Interactiable?.Interact(this);
        }
        if (dialogueUI.IsOpen)
        {
            dialoguePrompt.text = "";
        }
        if (isGrounded == false)
        {
            myAnim.SetBool("isJumping", true);
        }
        if (isGrounded)
        {
            myAnim.SetBool("isJumping", false);
        }
    }

    void Move()
    {
        if (myAnim.GetFloat("lastMoveX") == 0)
        {
            isRight = true;
            isLeft = false;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float moveBy = x * speed;
        if (cantMove)
        {

        }
        else
        {
            rb.velocity = new Vector2(moveBy, rb.velocity.y);
        }
        if(dialogueUI.IsOpen == true)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        myAnim.SetFloat("moveX", rb.velocity.x);

        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1)
        {
            myAnim.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
        }
        if (myAnim.GetFloat("lastMoveX") < -0.5 && isRight == true && dialogueUI.IsOpen == false && cantMove == false) //checks to see if player is facing left
        {
            Vector3 myVec = new Vector3(0f, 180f, 0f);
            gameObject.transform.Rotate(myVec);
            isRight = false;
            isLeft = true;
        }
        if (myAnim.GetFloat("lastMoveX") > 0.5 && isLeft == true && dialogueUI.IsOpen == false && cantMove == false) //checks to see if player is facing right
        {
            Vector3 myVec = new Vector3(0f, 180f, 0f);
            gameObject.transform.Rotate(myVec);
            isRight = true;
            isLeft = false;
        }
    }

    void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && isGrounded && dialogueUI.IsOpen == false && cantMove == false)
        {
            AudioSource.PlayClipAtPoint(jumpingSE, transform.position);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            //rb.velocity += Vector2.up * jumpForce;
            //myAnim.SetBool("isJumping", true);
        }
    }

    void CheckIfGrounded()
    {
        Collider2D collider = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer);
        if (collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
            isToyCar = true;

            SceneManager.LoadScene("Battle", LoadSceneMode.Additive);
            mainCamera.SetActive(false);
            //playerCharacter.SetActive(false); //commented out for now
            eventSystem.SetActive(false);
        }
        if (collision.gameObject.tag == "Toy Soldier Enemy")
        {
            Destroy(collision.gameObject);
            isToySoldier = true;

            SceneManager.LoadScene("Battle", LoadSceneMode.Additive);
            mainCamera.SetActive(false);
            //playerCharacter.SetActive(false); //commented out for now
            eventSystem.SetActive(false);
        }
        if(collision.gameObject.tag == "battleTutorial")
        {
            Destroy(collision.gameObject);
            SceneManager.LoadScene("Battle(Tutorial)", LoadSceneMode.Additive);
            mainCamera.SetActive(false);
            //playerCharacter.SetActive(false); //commented out for now
            eventSystem.SetActive(false);
        }
        if (collision.gameObject.tag == "bossTest")
        {
            SceneManager.LoadScene("BossBattle", LoadSceneMode.Additive);
            mainCamera.SetActive(false);
            //playerCharacter.SetActive(false); //commented out for now
            eventSystem.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "killBox")
        {
            transform.position = StartPosition;
        }

        if (collision.gameObject.tag == "LevelEnd")
        {
            SceneManager.LoadScene("EndGame");
            mainCamera.SetActive(false);
            gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "Collectible")
        {
            levelUpScript.currExp += 5;
            Debug.Log("Collectible Acquired!");
            Destroy(collision.gameObject);
            AudioSource.PlayClipAtPoint(coin, transform.position);
        }

        if (collision.gameObject.tag == "NPC" || collision.gameObject.tag == "ballNPC")
        {
            dialoguePrompt.text = "Talk (E)";
        }
        if (collision.gameObject.tag == "flavorText")
        {
            dialoguePrompt.text = "Observe (E)";
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "NPC" || collision.gameObject.tag == "ballNPC" || collision.gameObject.tag == "flavorText")
        {
            dialoguePrompt.text = "";
        }
    }

    public void resetBattle()
    {
        mainCamera.SetActive(true);
        playerCharacter.SetActive(true);
        eventSystem.SetActive(true);
        this.transform.position = new Vector2(311.25f, 19.75f);
        manager.health = manager.maxHealth;
    }
}
