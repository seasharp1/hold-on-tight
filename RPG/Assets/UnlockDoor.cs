using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    public GameObject doorLock;
    public Animator doorAnim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(doorLock == null)
        {
            doorAnim.SetBool("Unlocked", true);
            doorAnim.SetBool("UnlockedAgain", true);
        }
    }
}
