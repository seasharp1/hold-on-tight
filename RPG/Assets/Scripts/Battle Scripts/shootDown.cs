using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootDown : MonoBehaviour
{
    Rigidbody2D otherRB;
    public GameObject wireDialogue;
    // Start is called before the first frame update
    void Start()
    {
        otherRB = GameObject.Find("Hypno").GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "bullet")
        {
            wireDialogue.SetActive(true);
            otherRB.gravityScale = 1;
            Destroy(gameObject);
        }
    }
}
