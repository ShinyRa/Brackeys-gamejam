using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = System.Random;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject mushroomPrefab;
    public GameObject goblinPrefab;
    public GameObject evilWizard1Prefab;
    public GameObject skeletonPrefab;

    public GameObject playerGO;
    public GameObject enemyGO;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    public GameObject hotBar;

    BaseUnit playerUnit;
    BaseUnit enemyUnit;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleState state;

    private int currentLevel = 0;
    private int totalLevels = 3;

    public GameObject[] enemyLibrary;

    // Start is called before the first frame update
    void Start()
    {
        playerGO = SimplePool.Spawn(playerPrefab, playerBattleStation);
        Restart();
    }

    void Restart()
    {
        state = BattleState.START;
        hotBar.SetActive(false);
        StartCoroutine(SetupBattle(currentLevel));
    }

    IEnumerator SetupBattle(int level)
    {
        enemyLibrary = new GameObject[] { mushroomPrefab, goblinPrefab, skeletonPrefab, evilWizard1Prefab };

        enemyGO = SimplePool.Spawn(enemyLibrary[level], enemyBattleStation);

        playerUnit = playerGO.GetComponent<BaseUnit>();
        enemyUnit = enemyGO.GetComponent<BaseUnit>();

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
        yield return new WaitForSeconds(0.2f);
        enemyUnit.TakeDamage(playerUnit.damage);
        State currentState = enemyUnit.getState();
        yield return new WaitForSeconds(0.2f);
        enemyHUD.SetHP(enemyUnit.currentHP);
        yield return new WaitForSeconds(2f);

        if (currentState == State.DEAD)
        {
            currentState = State.ALIVE;
            SimplePool.Despawn(enemyGO);
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    public void OnAttackButton(string attackType)
    {
        if (state != BattleState.PLAYERTURN)
            return;
        playerUnit.DealAttack(attackType);
        hotBar.SetActive(false);
        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton(int amountHeal)
    {
        if (state != BattleState.PLAYERTURN)
            return;
        playerUnit.GainHealth(amountHeal);
        playerHUD.SetHP(playerUnit.currentHP);

        hotBar.SetActive(false);
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(0.5f);
        enemyUnit.DealAttack("basicAttack");
        yield return new WaitForSeconds(0.3f);
        playerUnit.TakeDamage(enemyUnit.damage);
        State currentState = playerUnit.getState();
        yield return new WaitForSeconds(0.3f);
        playerHUD.SetHP(playerUnit.currentHP);
        yield return new WaitForSeconds(1f);

        if (currentState == State.DEAD)
        {
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator EndBattle()
    {
        if (state == BattleState.WON)
        {
            Debug.Log("You WON");
            if (currentLevel == totalLevels)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
            }
            currentLevel++;
            enemyUnit.GainFullHealth();
            Restart();
        }
        else if (state == BattleState.LOST)
        {
            Debug.Log("You LOST");
            yield return new WaitForSeconds(2f);
            Destroy(playerGO);
            Destroy(enemyGO);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private int Random(int min, int max)
    {
        Random rnd = new Random();
        return rnd.Next(min, max);
    }
}
