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
}
