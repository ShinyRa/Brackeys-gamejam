using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using TMPro;
using Random = System.Random;
using UnityEngine.SceneManagement;

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

    private int currentLevel = 0;
    private int totalLevels = 3;

    private readonly string CHARACTER_DATA_FILE = "characterData";

    void Start()
    {
        this.ReadCharacterData();

        spellBook.SetActive(false);
        playerGO = Instantiate(playerPrefab, playerBattleStation);
        Restart();
    }

    /**
     * Read Json character data from location
     */
    private void ReadCharacterData()
    {
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

    void Restart()
    {
        state = BattleStateEnum.START;
        spellBook.SetActive(false);
        StartCoroutine(SetupBattle(currentLevel));
    }

    IEnumerator SetupBattle(int level)
    {
        enemyGO = Instantiate(enemyPrefabs[level], enemyBattleStation);

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

    IEnumerator PlayerAttack()
    {
        SoundManagerScript.PlaySound ("playerAttack");
        yield return new WaitForSeconds(0.2f);
        enemyUnit.TakeDamage(playerUnit.damage);
        State currentState = enemyUnit.getState();
        yield return new WaitForSeconds(0.2f);
        enemyHUD.SetHP(enemyUnit.currentHP);
        yield return new WaitForSeconds(2f);

        if (currentState == State.DEAD)
        {
            currentState = State.ALIVE;
            Destroy(enemyGO);
            state = BattleStateEnum.WON;
            StartCoroutine(EndBattle());
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
        yield return new WaitForSeconds(0.5f);
        enemyUnit.DealAttack("thirdAttack");
        yield return new WaitForSeconds(0.3f);
        playerUnit.TakeDamage(enemyUnit.damage);
        State currentState = playerUnit.getState();
        yield return new WaitForSeconds(0.3f);
        playerHUD.SetHP(playerUnit.currentHP);
        yield return new WaitForSeconds(1f);

        if (currentState == State.DEAD)
        {
            state = BattleStateEnum.LOST;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleStateEnum.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator EndBattle()
    {
        if (state == BattleStateEnum.WON)
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
        else if (state == BattleStateEnum.LOST)
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
