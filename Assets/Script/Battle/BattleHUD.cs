using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{

    public TextMeshProUGUI nameTag;
    public TextMeshProUGUI levelText;
    public Slider hpSlider;
    public Image unitIcon;

    public void SetHUD(BaseUnit unit)
    {
        nameTag.text = unit.unitName;
        levelText.text = "" + unit.unitLevel;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
        unitIcon.sprite = unit.unitIcon;
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }

}
