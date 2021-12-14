using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTrigger : MonoBehaviour
{
    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Jackyll")
        {
            yield return new WaitForSeconds(.2f);
            Destroy(gameObject);
        }
    }
}
