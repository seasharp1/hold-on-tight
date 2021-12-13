using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptManager : MonoBehaviour
{
    public int maxHealth;
    public int health;
    public int extraDamage;
    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
        maxHealth = 20;
        health = 20;
        extraDamage = 0;
    }
}
