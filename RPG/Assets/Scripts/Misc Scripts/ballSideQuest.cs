using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballSideQuest : MonoBehaviour
{
    public GameObject first;
    public GameObject second;
    public GameObject third;

    public bool questComplete = false;
    void Update()
    {
        if(first == null && second == null && third == null)
        {
            questComplete = true;
        }
    }
}
