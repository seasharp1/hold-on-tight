using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float speed = 3f;
    public bool isGoingRight = true;
    public float raycastDistance = 1f;

    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        sprite.flipX = isGoingRight;
    }

    // Update is called once per frame
    void Update()
    {
        // If enemy is going right, get vector pointing to its right
        Vector3 directionTranslation = (isGoingRight) ? transform.right : -transform.right;
        directionTranslation *= Time.deltaTime * speed;

        transform.Translate(directionTranslation);

        CheckForCollision();
    }

    private void CheckForCollision()
    {
        Vector3 raycastDirection = (isGoingRight) ? Vector3.right : Vector3.left;

        // Parameters are point of origin, direction, and max distance of raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position + raycastDirection * raycastDistance - new Vector3(0f, 0.25f, 0f), raycastDirection, 0.075f);

        // If colliding with ground object, change direction. Be sure to tag ground objects as "Ground"
        if (hit.collider != null)
        {
            if (hit.transform.tag == "Ground")
            {
                isGoingRight = !isGoingRight;
                sprite.flipX = isGoingRight;
            }
        }
    }
}
