using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floatUpAndDown : MonoBehaviour
{
    Vector2 floatY;
    Vector2 attachedPos;

    public float FloatStrength;

    public float yLevel;

    public GameObject attachedTo;
    // Update is called once per frame
    void Update()
    {
        attachedPos = attachedTo.transform.position;
        floatY = transform.position;
        floatY.y = (Mathf.Sin(Time.time) * FloatStrength) + attachedPos.y;
        transform.position = floatY;
    }
}
