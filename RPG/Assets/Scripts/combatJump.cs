using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combatJump : MonoBehaviour
{
    public Rigidbody2D rb;
    public bool isGrounded = false;
    public Transform isGroundedChecker;
    public float checkGroundRadius;
    public LayerMask groundLayer;
    public Animator myAnim;
    public float jumpForce;
    public AudioClip jumpingSE;

    GameObject dialogueHolder;
    DialogueUI dialogueUI;
    public bool canJump;
    // Start is called before the first frame update
    void Start()
    {
        canJump = true;
        dialogueHolder = GameObject.Find("Canvas");
        dialogueUI = dialogueHolder.GetComponent<DialogueUI>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        CheckIfGrounded();
        if(isGrounded == false)
        {
            myAnim.SetBool("isJumping", true);
        }
        if (isGrounded)
        {
            myAnim.SetBool("isJumping", false);
        }
    }
    void Jump()  
    {
        if (Input.GetKey(KeyCode.Space) && isGrounded && dialogueUI.IsOpen == false && canJump)
        {
            canJump = false;
            AudioSource.PlayClipAtPoint(jumpingSE, transform.position);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            //yield return new WaitForSeconds(1f);
            canJump = true;
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
}
