using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { ALIVE, DEAD }

public class Unit : MonoBehaviour
{

    public string unitName;
    public int unitLevel;

    public int damage;

    public int maxHP;
    public int currentHP;

    public Animator ani;

    public State state = State.ALIVE;

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

    public bool GainHealth(int hp)
    {
        currentHP += hp;

        if (currentHP == maxHP)
            return true;
        else
            return false;
    }

    public State getState()
    {
        return state;
    }
}
