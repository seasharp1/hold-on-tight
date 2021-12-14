using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lowerVolume : MonoBehaviour
{
    public GameObject optionsMenu;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(onAndOff());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator onAndOff()
    {
        optionsMenu.SetActive(true);
        yield return null;
        optionsMenu.SetActive(false);
    }
}
