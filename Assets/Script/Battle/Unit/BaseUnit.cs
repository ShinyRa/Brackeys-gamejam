using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitState { ALIVE, POISIONED, DEAD, WON }

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit")]
public class BaseUnit : ScriptableObject
{
    public string unitName;
    public GameObject prefab;
    public float health;
    public List<Action> actions;
    public UnitState state = UnitState.ALIVE;
    public float damage = 0f;

    void Start() {
        this.damage = 0f;
    }


    // public void Animate(Action action) {
    //     this.ani.SetTrigger(action.actionName);
    // }

    public void ResolveAction(Action action) {
        // this.Animate(action);
        this.damage += action.CalculateDamage();

        if (IsDead()) {
            // this.prefab.GetComponent<Animator>().SetBool("isDeath", true);
            state = UnitState.DEAD;
        }
    }

    public float getHpRemaining() {
        return (this.health - this.damage);
    }

    public bool IsDead() {
        return this.getHpRemaining() < 0f;
    }
}
