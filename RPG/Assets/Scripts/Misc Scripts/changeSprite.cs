using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeSprite : MonoBehaviour
{
    SpriteRenderer currentSprite;
    public Sprite[] spriteArray;
    public bool change = false;
    // Start is called before the first frame update
    void Start()
    {
        currentSprite = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (change)
        {
            currentSprite.sprite = spriteArray[0];
        }
        else
        {
            currentSprite.sprite = spriteArray[1];
        }
    }
}
