using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combatJump : MonoBehaviour
{
    Rigidbody2D rb;
    public bool isGrounded = false;
    public Transform isGroundedChecker;
    public float checkGroundRadius;
    public LayerMask groundLayer;
    public Animator myAnim;
    public float jumpForce;
    public AudioClip jumpingSE;

    BattleSystem battle;
    // Start is called before the first frame update
    void Start()
    {
        battle = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        CheckIfGrounded();
    }
    void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && isGrounded && battle.state == BattleState.ENEMYTURN)
        {
            AudioSource.PlayClipAtPoint(jumpingSE, transform.position);
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
}
