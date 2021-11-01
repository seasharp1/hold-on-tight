using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptManager : MonoBehaviour
{
    public int maxHealth;
    public int health;
    public int level;
    public int healthModifier = 0;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        level = PlayerController.playerCharacter.GetComponent<LevelUpSystem>().currLevel;

        if(level > 1)
        {
            healthModifier += (5 * level);
        }

        maxHealth = 50 + healthModifier;
        health = 50 + healthModifier;
    }
}
