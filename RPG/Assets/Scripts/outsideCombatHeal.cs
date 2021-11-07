using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outsideCombatHeal : MonoBehaviour
{
    ScriptManager scriptManager;
    bool isTriggered = false;
    private void Start()
    {
        scriptManager = GameObject.FindWithTag("PlayerGameManager").GetComponent<ScriptManager>();
    }
    private void Update()
    {
        if (isTriggered == true && Input.GetKeyDown(KeyCode.E))
        {
            scriptManager.health = scriptManager.maxHealth;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isTriggered = true;
        }
    }
}
