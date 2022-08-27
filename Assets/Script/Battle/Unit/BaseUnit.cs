using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { ALIVE, POISIONED, DEAD, WON }

public class BaseUnit : MonoBehaviour
{
    public string unitName { get; set; }
    public string prefabName { get; set; }
    public float health { get; set; }
    public List<Action> actions { get; set; }
    public float damage = 0f;
    public State state { get; set; } = State.ALIVE;

    /**
     * Initialize a new unit based on parsed UnitData
     */
    public BaseUnit(UnitData unit) {
        this.Build(unit);
    }

    public void Build(UnitData unit) {
        (string _name, float _health, string _prefabName, List<ActionData> _actions) = unit;
        this.unitName = _name;
        this.health = _health;
        this.prefabName = _prefabName;
        for (int i = 0; i < _actions.Count; i++)
        {
            this.actions.Add(new Action(_actions[i]));
        }
    }
    // public void Animate(Action action) {
    //     this.animation.SetTrigger(action.animationClip);
    // }

    public void ResolveAction(Action action) {
        // this.Animate(action);
        this.damage += action.CalculateDamage();

        if (IsDead()) {
            // this.animation.SetBool("isDeath", true);
            state = State.DEAD;
        }
    }

    public float getHpRemaining() {
        return (this.health - this.damage);
    }

    private bool IsDead() {
        return this.getHpRemaining() < 0f;
    }

    // public void DealAttack(string attackType)
    // {
    //     ani.SetTrigger(attackType);
    // }

    // public void TakeDamage(int dmg)
    // {
    //     ani.SetTrigger("takeHit");
    //     currentHP -= dmg;

    //     if (currentHP <= 0)
    //     {
    //         ani.SetBool("isDeath", true);
    //         state = State.DEAD;
    //     }
    // }

    // public void GainHealth(int hp)
    // {
    //     currentHP = currentHP + hp;

    //     ani.SetTrigger("heal");

    //     if (currentHP >= maxHP)
    //     {
    //         currentHP = maxHP;
    //     }
    // }
}
