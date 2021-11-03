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
    int healthModifier = 0;

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
            unit.maxHP += ModifyHP();
            unit.currentHP += ModifyHP();
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
        if (isPlayer)
        {
            hp += ModifyHP();
        }

        HPText.text = "HP: " + hp + "/" + unit.maxHP;
        hpSlider.value = hp;
    }

    public int ModifyHP()
    {
        int level = PlayerController.playerCharacter.GetComponent<LevelUpSystem>().currLevel;

        if (level > 1)
        {
            healthModifier = (5 * level);
            return healthModifier;
        }
        else
        {
            return 0;
        }
    }
}
