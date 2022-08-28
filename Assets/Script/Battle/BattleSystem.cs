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


    public SoundManagerScript sound;
    private int amountOfDamage;
    public GameObject healButton;

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
        healButton.SetActive(true);
        StartCoroutine(SetupBattle(currentLevel));
    }

    IEnumerator SetupBattle(int level)
    {
        enemyGO = Instantiate(enemyPrefabs[level], enemyBattleStation);

        playerUnit = playerGO.GetComponent<BaseUnit>();
        enemyUnit = enemyGO.GetComponent<BaseUnit>();

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(0f);

        state = BattleStateEnum.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        spellBook.SetActive(true);
    }

    IEnumerator PlayerAttack(string attackType, string attackSound)
    {

        yield return new WaitForSeconds(0.1f);
        sound.PlaySound(attackSound);
        yield return new WaitForSeconds(0.2f);
        if (attackType == "waveAttack")
        {
            enemyUnit.TakeDamage(playerUnit.damage + 3);
        } else
        {
            enemyUnit.TakeDamage(playerUnit.damage);
        }

        State currentState = enemyUnit.getState();
        yield return new WaitForSeconds(0.2f);
        enemyHUD.SetHP(enemyUnit.currentHP);
        yield return new WaitForSeconds(1f);

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
        
        sound.PlaySound("UIClickSound");
        playerUnit.DealAttack(attackType);
        playerHUD.SetHP(playerUnit.currentHP);
        spellBook.SetActive(false);
        StartCoroutine(PlayerAttack(attackType, Random(0, 1) == 0 ? "playerAttack" : "playerAttack2"));
    }

    public void OnHealButton(int amountHeal)
    {
        if (state != BattleStateEnum.PLAYERTURN)
            return;
        sound.PlaySound("UIClickSound");
        playerUnit.GainFullHealth();
        playerHUD.SetHP(playerUnit.currentHP);
        sound.PlaySound("playerHeal");
        healButton.SetActive(false);
        spellBook.SetActive(false);
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(0.5f);
        if (enemyUnit.name == "Evil Wizard 1(Clone)")
        {
            string randomdAttack = getEvilAttack(Random(0,2));
            if (randomdAttack == "slamAttack") {
                amountOfDamage = enemyUnit.damage;
            } else if (randomdAttack == "waveAttack"){
                amountOfDamage = enemyUnit.damage + 2;
            }
            enemyUnit.DealAttack(randomdAttack);
            enemyHUD.SetHP(enemyUnit.currentHP);
            
        } else {
            string randomdAttack = getAttack(Random(0,3));
            enemyUnit.DealAttack(randomdAttack);

            if (randomdAttack == "basicAttack") {
                amountOfDamage = enemyUnit.damage + 1;
            } else if (randomdAttack == "secondAttack"){
                amountOfDamage = enemyUnit.damage + 2;
            } else if (randomdAttack == "thirdAttack"){
                amountOfDamage = enemyUnit.damage + 3;
            }
        }
        
        int randomInt = Random(0, 2);
        if (randomInt == 0) {
            sound.PlaySound("playerTakeHit");
        } else {
            sound.PlaySound("playerTakeHit" + randomInt);
        }
        yield return new WaitForSeconds(0.3f);
        playerUnit.TakeDamage(amountOfDamage);
        State currentState = playerUnit.getState();

        yield return new WaitForSeconds(0.3f);

        playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if (currentState == State.DEAD)
        {
            sound.PlaySound("playerDeath");
            state = BattleStateEnum.LOST;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleStateEnum.PLAYERTURN;
            PlayerTurn();
        }
        yield return new WaitForSeconds(0.3f);
        // yield return new WaitForSeconds(0.3f);
        playerHUD.SetHP(playerUnit.currentHP);
        // yield return new WaitForSeconds(1f);

        
    }

    private string getAttack(int randomint)
    {
        switch (randomint)
        {
            case 0:
            return "basicAttack";
            case 1:
            return "secondAttack";
            case 2:
            return "thirdAttack";
            default:
            return "basicAttack";
        }
    }

    private string getEvilAttack(int randomint)
    {
        switch (randomint)
        {
            case 0:
            return "slamAttack";
            case 1:
            return "waveAttack";
            default:
            return "slamAttack";
        }
    }

    IEnumerator EndBattle()
    {
        if (state == BattleStateEnum.WON)
        {
            if (currentLevel == totalLevels)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
            } else {
                currentLevel++;
            }
            enemyUnit.GainFullHealth();
            Restart();
        }
        else if (state == BattleStateEnum.LOST)
        {
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
