using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floatUpAndDown : MonoBehaviour
{
    Vector2 floatY;
    Vector2 attachedPos;

    public float FloatStrength;

    public float yLevelAdjustment;

    public GameObject attachedTo;

    public bool stopMoving = false;
    // Update is called once per frame
    void Update()
    {
        if (stopMoving)
        {

        }
        else
        {
            attachedPos = attachedTo.transform.position;
            floatY = transform.position;
            floatY.y = (Mathf.Sin(Time.time) * FloatStrength) + (attachedPos.y + yLevelAdjustment);
            transform.position = floatY;
        }

    }
}
