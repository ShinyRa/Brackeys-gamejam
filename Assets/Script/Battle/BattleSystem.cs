using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using TMPro;
using Random = System.Random;

public class BattleSystem : MonoBehaviour
{
    /**
     * JSon Character Data reader
     */
    public CharacterDataReader reader;

    /**
     * Prefab references
     */
    public GameObject playerPrefab;
    public List<GameObject> enemyPrefabs;

    /**
     * Battlestation locations to spawn player / enemies
     */
    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    /**
     * Player menu to choose spells
     */
    public GameObject spellBook;

    /**
     * Unit datastructure
     */
    BaseUnit playerUnit;
    BaseUnit enemyUnit;

    /**
     * Player / Enemey references
     */
    public GameObject playerGO;
    public GameObject enemyGO;

    /**
     * Battle HUD references
     */
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    /**
     * State of battle
     */
    public BattleStateEnum state = BattleStateEnum.START;

    private int level = 0;

    private readonly string CHARACTER_DATA_FILE = "characterData";

    void Start()
    {
        this.ReadCharacterData();

        spellBook.SetActive(false);
        playerGO = SimplePool.Spawn(playerPrefab, playerBattleStation);
        StartCoroutine(SetupBattle(level));
    }

    /**
     * Read Json character data from location
     */ 
    private void ReadCharacterData() {
        TextAsset characterData = Resources.Load(CHARACTER_DATA_FILE) as TextAsset;
        this.reader = new CharacterDataReader(characterData);
        CharacterData data = this.reader.GetList();
        // this.characterData = this.reader.GetList();

        // this.characterData.units

        // Action[] actions = {new Action(), new Action(), new Action(), new Action()};
        // spellBook.SetActions(actions);
        for (int i = 0; i < data.units.Count; i++)
        {
            // Debug.Log("Assets/Prefabs/Characters/" + data.units[i].prefabName);
            // GameObject test = Instantiate(Resources.Load("Assets/Prefabs/Characters/" + data.units[i].prefabName, typeof(GameObject))) as GameObject;
            // this.unitPrefabs.Add();
        }       
    } 

    void NextLevel()
    {
        state = BattleStateEnum.START;
        spellBook.SetActive(false);
        enemyUnit.GainFullHealth();
        StartCoroutine(SetupBattle(level));
    }

    IEnumerator SetupBattle(int level)
    {
        enemyGO = SimplePool.Spawn(enemyPrefabs[level], enemyBattleStation);

        playerUnit = playerGO.GetComponent<BaseUnit>();
        enemyUnit = enemyGO.GetComponent<BaseUnit>();

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(1f);

        state = BattleStateEnum.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        spellBook.SetActive(true);
    }

    public void OnDodge()
    {
        Debug.Log("Dodging");
    }

    IEnumerator PlayerAttack()
    {
        enemyUnit.TakeDamage(playerUnit.damage);
        State currentState = enemyUnit.getState();

        enemyHUD.SetHP(enemyUnit.currentHP);

        yield return new WaitForSeconds(2f);

        if (currentState == State.DEAD)
        {
            SimplePool.Despawn(enemyGO);
            state = BattleStateEnum.WON;
            EndBattle();
        }
        else
        {
            state = BattleStateEnum.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    public void OnAttackButton(string attackType)
    {
        if (state != BattleStateEnum.PLAYERTURN)
            return;
        playerUnit.DealAttack(attackType);
        spellBook.SetActive(false);
        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton(int amountHeal)
    {
        if (state != BattleStateEnum.PLAYERTURN)
            return;

        playerUnit.GainHealth(amountHeal);
        playerHUD.SetHP(playerUnit.currentHP);

        spellBook.SetActive(false);
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        spellBook.SetActive(false);
        yield return new WaitForSeconds(1f);

        enemyUnit.DealAttack("basicAttack");

        playerUnit.TakeDamage(enemyUnit.damage);
        State currentState = playerUnit.getState();

        playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if (currentState == State.DEAD)
        {
            state = BattleStateEnum.LOST;
            EndBattle();
        }
        else
        {
            state = BattleStateEnum.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        Random rnd = new Random();
        if (state == BattleStateEnum.WON)
        {
            Debug.Log("You WON");
            // level = rnd.Next(0, enemyLibrary.Length);
            NextLevel();
        }
        else if (state == BattleStateEnum.LOST)
        {
            Debug.Log("You LOST");
        }

    }
}
