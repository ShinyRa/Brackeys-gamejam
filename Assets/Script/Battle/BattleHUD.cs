using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{

    public TextMeshProUGUI nameTag;
    // public TextMeshProUGUI levelText;
    public Slider hpSlider;
    public Image unitIcon;

    public void SetHUD(BaseUnit unit)
    {
        nameTag.text = unit.unitName;
        // levelText.text = "" + unit.unitLevel;
        hpSlider.maxValue = unit.health;
        hpSlider.value = (unit.health - unit.damage);
        // unitIcon.sprite = unit.unitIcon;
    }

    public void SetHP(float hp)
    {
        hpSlider.value = hp;
    }
}
