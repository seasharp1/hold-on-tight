using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public Vector3 newLocation;
    public GameObject player;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            player.transform.position = newLocation;
        }
    }
}
