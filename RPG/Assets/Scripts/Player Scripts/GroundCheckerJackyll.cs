using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckerJackyll : MonoBehaviour
{
    public bool isGrounded = false;
    public Transform isGroundedChecker;
    public float checkGroundRadius;
    public LayerMask groundLayer;

    public Animator anim;

    bool finish = false;

    // Update is called once per frame
    void Update()
    {
        CheckIfGrounded();
        if (isGrounded && finish == false)
        {
            finish = true;
            anim.SetBool("isBoing", true);
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
