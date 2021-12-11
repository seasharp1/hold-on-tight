using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakFloor : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "hypnoBreak" )
        {
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "breakHypno")
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }
}
