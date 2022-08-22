using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    public GameObject hotBar;

    Unit playerUnit;
    Unit enemyUnit;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        hotBar.SetActive(false);
        GameObject playerGO = SimplePool.Spawn(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();
        GameObject enemyGO = SimplePool.Spawn(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        hotBar.SetActive(true);
    }

    IEnumerator PlayerAttack()
    {
        enemyUnit.TakeDamage(playerUnit.damage);
        State currentState = enemyUnit.getState();

        enemyHUD.SetHP(enemyUnit.currentHP);

        yield return new WaitForSeconds(2f);

        if (currentState == State.DEAD)
        {
            SimplePool.Despawn(enemyPrefab);
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        hotBar.SetActive(false);
        StartCoroutine(PlayerAttack());
    }

    IEnumerator EnemyTurn()
    {
        hotBar.SetActive(false);
        yield return new WaitForSeconds(1f);

        playerUnit.TakeDamage(enemyUnit.damage);
        State currentState = playerUnit.getState();

        playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if (currentState == State.DEAD)
        {
            SimplePool.Despawn(playerPrefab);
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            Debug.Log("You WON");
        }
        else if (state == BattleState.LOST)
        {
            Debug.Log("You LOST");
        }
    }
}
