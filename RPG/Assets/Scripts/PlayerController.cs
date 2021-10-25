using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Vector3 StartPosition;
    public Vector3 CombatLastLocation;
    Rigidbody2D rb;
    public float speed;
    public float jumpForce;
    bool isGrounded = false;
    public Transform isGroundedChecker;
    public float checkGroundRadius;
    public LayerMask groundLayer;

    public static GameObject mainCamera;
    //public static GameObject playerCharacter;


    public Animator myAnim;

    SpriteRenderer myRender;
    bool isLeft = false;
    bool isRight = true;

    public static PlayerController instance;
    public string areaTransitionName;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        //playerCharacter = GameObject.FindGameObjectWithTag("Player");


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
        rb = GetComponent<Rigidbody2D>();
        StartPosition = transform.position;
        myRender = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        CheckIfGrounded();
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
        rb.velocity = new Vector2(moveBy, rb.velocity.y);
        myAnim.SetFloat("moveX", rb.velocity.x);
        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1)
        {
            myAnim.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
        }
        if (myAnim.GetFloat("lastMoveX") < -0.5 && isRight == true) //checks to see if player is facing left
        {
            Vector3 myVec = new Vector3(0f, 180f, 0f);
            gameObject.transform.Rotate(myVec);
            isRight = false;
            isLeft = true;
        }
        if (myAnim.GetFloat("lastMoveX") > 0.5 && isLeft == true) //checks to see if player is facing right
        {
            Vector3 myVec = new Vector3(0f, 180f, 0f);
            gameObject.transform.Rotate(myVec);
            isRight = true;
            isLeft = false;
        }
    }
    void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            myAnim.SetBool("isJumping", true);
        }
    }
    void CheckIfGrounded()
    {
        Collider2D collider = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer);
        if (collider != null)
        {
            isGrounded = true;
            myAnim.SetBool("isJumping", false);
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
            //save position
            CombatLastLocation = transform.position;
            Destroy(collision.gameObject);
            SceneManager.LoadScene("Battle");
            mainCamera.SetActive(false);
            //playerCharacter.SetActive(false);
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
    }
}
