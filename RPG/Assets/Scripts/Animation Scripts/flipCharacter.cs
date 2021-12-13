using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flipCharacter : MonoBehaviour
{
    Animator playerAnim;

    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GameObject.FindWithTag("Player").GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (playerAnim.GetFloat("lastMoveX") < -0.5)
            {
                print("flipped");
                Vector3 myVec = new Vector3(0f, 180f, 0f);
                GameObject.FindWithTag("Player").transform.Rotate(myVec);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
