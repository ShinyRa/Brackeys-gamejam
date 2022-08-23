using System.Collections;
using UnityEngine;
using TMPro;
using Random = System.Random;

// public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public CharacterDataReader reader;
    // public GameObject playerPrefab;
    // public GameObject mushroomPrefab;
    // public GameObject goblinPrefab;
    // public GameObject evilWizard1Prefab;
    // public GameObject skeletonPrefab;

    // public GameObject playerGO;
    // public GameObject enemyGO;

    // public Transform playerBattleStation;
    // public Transform enemyBattleStation;

    // public GameObject hotBar;

    // BaseUnit playerUnit;
    // BaseUnit enemyUnit;

    // public BattleHUD playerHUD;
    // public BattleHUD enemyHUD;

    // public BattleState state;

    // private int level = 0;

    // public GameObject[] enemyLibrary;

    // Start is called before the first frame update

    private readonly string CHARACTER_DATA_FILE = "characterData";
    void Start()
    {
        TextAsset characterData = Resources.Load(CHARACTER_DATA_FILE) as TextAsset;

        this.reader = new CharacterDataReader(characterData);
        Debug.Log(this.reader.GetList().units[0].name);
        Debug.Log(this.reader.GetList().units[0].prefabName);
        Debug.Log("Actions: ");
        for (int i = 0; i < this.reader.GetList().units[0].actions.Length; i++)
        {
            Debug.Log(this.reader.GetList().units[0].actions[i].name);
            Debug.Log(this.reader.GetList().units[0].actions[i].healthModifier);
            Debug.Log(this.reader.GetList().units[0].actions[i].margin);
            Debug.Log(this.reader.GetList().units[0].actions[i].successRate);
        }

        // state = BattleState.START;
        // hotBar.SetActive(false);
        // playerGO = SimplePool.Spawn(playerPrefab, playerBattleStation);
        // StartCoroutine(SetupBattle(level));
    }

    // void NextLevel()
    // {
    //     state = BattleState.START;
    //     hotBar.SetActive(false);
    //     enemyUnit.GainFullHealth();
    //     StartCoroutine(SetupBattle(level));
    // }

    // IEnumerator SetupBattle(int level)
    // {
    //     enemyLibrary = new GameObject[] { mushroomPrefab, goblinPrefab, skeletonPrefab, evilWizard1Prefab };

    //     enemyGO = SimplePool.Spawn(enemyLibrary[level], enemyBattleStation);

    //     playerUnit = playerGO.GetComponent<BaseUnit>();
    //     enemyUnit = enemyGO.GetComponent<BaseUnit>();

    //     playerHUD.SetHUD(playerUnit);
    //     enemyHUD.SetHUD(enemyUnit);

    //     yield return new WaitForSeconds(2f);

    //     state = BattleState.PLAYERTURN;
    //     PlayerTurn();
    // }

    // void PlayerTurn()
    // {
    //     hotBar.SetActive(true);
    // }

    // public void OnDodge()
    // {
    //     Debug.Log("Dodging");
    // }

    // IEnumerator PlayerAttack()
    // {
    //     enemyUnit.TakeDamage(playerUnit.damage);
    //     State currentState = enemyUnit.getState();

    //     enemyHUD.SetHP(enemyUnit.currentHP);

    //     yield return new WaitForSeconds(2f);

    //     if (currentState == State.DEAD)
    //     {
    //         SimplePool.Despawn(enemyGO);
    //         state = BattleState.WON;
    //         EndBattle();
    //     }
    //     else
    //     {
    //         state = BattleState.ENEMYTURN;
    //         StartCoroutine(EnemyTurn());
    //     }
    // }

    // public void OnAttackButton(string attackType)
    // {
    //     if (state != BattleState.PLAYERTURN)
    //         return;
    //     playerUnit.DealAttack(attackType);
    //     hotBar.SetActive(false);
    //     StartCoroutine(PlayerAttack());
    // }

    // public void OnHealButton(int amountHeal)
    // {
    //     if (state != BattleState.PLAYERTURN)
    //         return;

    //     playerUnit.GainHealth(amountHeal);
    //     playerHUD.SetHP(playerUnit.currentHP);

    //     hotBar.SetActive(false);
    //     StartCoroutine(EnemyTurn());
    // }

    // IEnumerator EnemyTurn()
    // {
    //     hotBar.SetActive(false);
    //     yield return new WaitForSeconds(1f);

    //     enemyUnit.DealAttack("basicAttack");

    //     playerUnit.TakeDamage(enemyUnit.damage);
    //     State currentState = playerUnit.getState();

    //     playerHUD.SetHP(playerUnit.currentHP);

    //     yield return new WaitForSeconds(1f);

    //     if (currentState == State.DEAD)
    //     {
    //         state = BattleState.LOST;
    //         EndBattle();
    //     }
    //     else
    //     {
    //         state = BattleState.PLAYERTURN;
    //         PlayerTurn();
    //     }
    // }

    // void EndBattle()
    // {
    //     Random rnd = new Random();
    //     if (state == BattleState.WON)
    //     {
    //         Debug.Log("You WON");
    //         level = rnd.Next(0, enemyLibrary.Length);
    //         NextLevel();
    //     }
    //     else if (state == BattleState.LOST)
    //     {
    //         Debug.Log("You LOST");
    //     }

    // }
}
