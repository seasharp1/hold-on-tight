using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Text nameText;
    public Text levelText;
    public Text HPText;
    public Slider hpSlider; 
    public Text damageText;
    //int healthModifier = 0;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "Lvl " + unit.unitLevel;
        HPText.text = "HP: " + unit.currentHP + "/" + unit.maxHP;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
    }

    public void SetHP(int hp, Unit unit)
    {
        HPText.text = "HP: " + hp + "/" + unit.maxHP;
        hpSlider.value = hp;
    }

    public void SetHUD_Start(Unit unit, bool isPlayer)
    {
        if (isPlayer)
        {
            // Modify values by current level of player
            unit.unitLevel = PlayerController.playerCharacter.GetComponent<LevelUpSystem>().currLevel;
        }

        // Set values
        nameText.text = unit.unitName;
        levelText.text = "Lvl " + unit.unitLevel;
        HPText.text = "HP: " + unit.currentHP + "/" + unit.maxHP;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
    }

    public void SetHP_Start(int hp, Unit unit, bool isPlayer)
    {
        HPText.text = "HP: " + hp + "/" + unit.maxHP;
        hpSlider.value = hp;
    }

}
