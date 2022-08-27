using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum State { ALIVE, POISIONED, DEAD, WON }


public class BaseUnit : MonoBehaviour
{

    // public GameObject tagPrefab;

    // public GameObject UnitTagGO;

    // public TextMeshProUGUI text;

    // PlayerTagScript playerTag;

    public string unitName;
    public int unitLevel;
    public Sprite unitIcon;

    public int damage;

    public int maxHP;
    public int currentHP;

    public Animator ani;
    public GameObject projectileGO;
    public GameObject projectilePrefab;

    public string[] attackTypes;

    public State state = State.ALIVE;

    public State getState() { return state; }

    public void GainFullHealth()
    {
        currentHP = maxHP;
    }

    public void DealAttack(string attackType)
    {
        ani.SetTrigger(attackType);
        if(attackType == "thirdAttack")
        {
            projectileGO = Instantiate(projectilePrefab);
        }
    }

    public void TakeDamage(int dmg)
    {
        ani.SetTrigger("takeHit");
        currentHP -= dmg;

        if (currentHP <= 0)
        {
            ani.SetBool("isDeath", true);
            state = State.DEAD;
        }
    }

    public void GainHealth(int hp)
    {
        currentHP = currentHP + hp;

        ani.SetTrigger("heal");

        if (currentHP >= maxHP)
        {
            currentHP = maxHP;
        }
    }
}
