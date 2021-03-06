using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpSystem : MonoBehaviour
{
    public Transform myTransform;
    //public Transform textPrefab;
    public Rect windowRect = new Rect(20, Screen.height / 2, 210, 125);

    public int maxExp;
    public int expToLevel = 0;
    public int currExp;
    public int dynamicNeededExp;
    public int staticNeededExp;
    public TextMeshProUGUI playerLevel;

    public int maxLevel = 20;
    public int currLevel = 1;
    private string displayLevel;
    public bool changeLevel = true;

    #region ProgressBar
    public bool expBarEnabled = true;
    public float expBarLength;
    public ScriptManager scriptManager;
    #endregion

    //public void instantiateText()
    //{
    //Instantiate(textPrefab, myTransform.position, myTransform.rotation);
    //}

    // Start is called before the first frame update
    void Start()
    {
        scriptManager = GameObject.Find("GameManager").GetComponent<ScriptManager>();
        //if (!textPrefab)
        //{
        //Debug.Log("Add 3DTextPrefab");
        //}
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        myTransform = go.transform;
    }

    /*
    private void OnGUI()
    {
        windowRect = GUI.Window(2, windowRect, LevelWdw2Function, "Display Level");

        if (expBarEnabled == true)
        {
            GUI.Box(new Rect(20, Screen.height - 475, expBarLength, 20), currExp + "/" + maxExp);
        }
    }

    void LevelWdw2Function(int windowID)
    {
        GUI.Box(new Rect(5, 20, 200, 100), displayLevel);
        GUI.DragWindow();
    }
    */

    // Update is called once per frame
    void Update()
    {
        playerLevel.text = "Level: " + currLevel +
            "\nExp to Level: " + dynamicNeededExp;
        /*
        displayLevel = "Current Level: " + currLevel +
            "\nExp to Level: " + expToLevel +
            "\nCurrent Exp: " + currExp +
            "\nExp Needed: " + dynamicNeededExp;
        */
        levelModifier();

        expBarLength = (Screen.width - 40) * ((currExp - expToLevel + staticNeededExp) / (float)staticNeededExp);
    }

    public void levelModifier()
    {
        if (changeLevel)
        {
            for (int i = 0; i < currLevel; ++i)
            {
                expToLevel += ((currLevel + 1) * 10);
                staticNeededExp = expToLevel - currExp;
                changeLevel = false;
            }
        }

        dynamicNeededExp = expToLevel - currExp;
        maxExp = expToLevel;

        if (currExp >= expToLevel && currLevel < maxLevel)
        {
            ++currLevel;
            scriptManager.maxHealth += 5;
            scriptManager.health += 5;
            scriptManager.extraDamage += 1;
            changeLevel = true;
            bool levelUp = true;

            if (levelUp)
            {
                //Text text = textPrefab.GetComponent<Text>();
                //text.text = "Level Up!";
                //instantiateText();
                levelUp = false;
            }
        }

        #region Exp
        if (currExp > maxExp)
        {
            currExp = maxExp;
        }
        if (currLevel > maxLevel)
        {
            currLevel = maxLevel;
        }
        if (currLevel < 1)
        {
            currLevel = 1;
        }
        #endregion
    }
}

