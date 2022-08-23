using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum State { ALIVE, POISIONED, DEAD }


public class Unit : MonoBehaviour
{

    // public GameObject tagPrefab;

    // public GameObject UnitTagGO;

    // public TextMeshProUGUI text;

    // PlayerTagScript playerTag;

    public string unitName;
    public int unitLevel;

    public int damage;

    public int maxHP;
    public int currentHP;

    public Animator ani;

    public string[] attackTypes;

    public State state = State.ALIVE;

    public State getState() { return state; }

    void Start()
    {

    }

    public void GainFullHealth()
    {
        currentHP = maxHP;
    }

    public void DealAttack(string attackType)
    {
        ani.SetTrigger(attackType);
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        ani.SetTrigger("Take hit");

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
