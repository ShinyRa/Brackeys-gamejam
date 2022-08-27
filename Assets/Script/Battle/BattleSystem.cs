using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BattleSystem : MonoBehaviour
{
    /**
     * Json character data reader
     */
    public CharacterDataReader reader;

    /**
     * Parsed data from character data Json reader
     */
    private CharacterData characterData;

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
    private int totalLevels = 4;

    private readonly string CHARACTER_DATA_FILE = "characterData";

    // Contructor
    void Start()
    {
        this.ReadCharacterData();
        spellBook.SetActive(false);
        playerGO = SimplePool.Spawn(playerPrefab, playerBattleStation);
        Restart();
    }

    /**
     * Read Json character data from location
     */
    private void ReadCharacterData()
    {
        TextAsset characterData = Resources.Load(CHARACTER_DATA_FILE) as TextAsset;
        this.reader = new CharacterDataReader(characterData);
        this.characterData = this.reader.GetParsed();
    }

    private void Restart()
    {
        state = BattleStateEnum.START;
        spellBook.SetActive(false);
        StartCoroutine(SetupBattle(currentLevel));
    }

    /**
     * Setup player / enemy states before start of battle.
     *
     * @param int level
     *
     * @return IEnumerator
     */
    private IEnumerator SetupBattle(int level)
    {
        this.Spawn(enemyPrefabs[currentLevel]);
        this.playerUnit = this.BuildBaseUnit(this.characterData.units.Find((unit) => unit.prefabName == "Player"));
        this.enemyUnit = this.BuildBaseUnit(this.characterData.units.Find((unit) => unit.prefabName == "Skeleton"));

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(1f);

        state = BattleStateEnum.PLAYERTURN;
        // PlayerTurn();
    }

    private BaseUnit BuildBaseUnit(UnitData data) {
        GameObject baseUnit = new GameObject("BaseUnit");
        baseUnit.SetActive(false);
        BaseUnit unit = baseUnit.AddComponent<BaseUnit>();
        unit = new BaseUnit(data);
        baseUnit.SetActive(true);
        return unit;
    }

    private void PlayerTurn()
    {
        Debug.Log("PLAYER");
        spellBook.SetActive(true);
    }

    private IEnumerator PlayerAttack(Action action)
    {
        yield return new WaitForSeconds(0.2f);
        enemyUnit.ResolveAction(action);
        State currentState = enemyUnit.state;
        yield return new WaitForSeconds(0.2f);
        enemyHUD.SetHP(enemyUnit.getHpRemaining());
        yield return new WaitForSeconds(2f);

        if (currentState == State.DEAD)
        {
            currentState = State.ALIVE;
            SimplePool.Despawn(enemyGO);
            state = BattleStateEnum.WON;
            StartCoroutine(EndBattle());
        }
        else
        {
            // state = BattleStateEnum.ENEMYTURN;
            // StartCoroutine(EnemyTurn());
        }
    }

    /**
     * Execute a player action based on action name
     *
     * @param string actionName
     */
    public void PlayerAct(string actionName) {
        if (state != BattleStateEnum.PLAYERTURN)
            return;

            Debug.Log("ACT!");

        Action action = this.FindActionOfUnit(this.playerUnit, actionName);
        Animator animation = playerGO.GetComponentInChildren<Animator>();
        animation.SetTrigger(action.actionName);
        spellBook.SetActive(false);
        StartCoroutine(PlayerAttack(action));
    }


    /**
     * Find action by name and unit.
     *
     * @param BaseUnit unit      unit where to find the action
     * @param string actionName  name of the action
     *
     * @return Action
     */
    private Action FindActionOfUnit(BaseUnit prefab, string actionName) {
        return prefab.actions.Find((action) => action.actionName == actionName);
    }

    /**
     * Spawn enemy into battle.
     *
     * @param GameObject enemyPrefab
     */
    private void Spawn(GameObject enemyPrefab)
    {
        enemyGO = SimplePool.Spawn(enemyPrefab, enemyBattleStation);
    }

    // public void OnHealButton(int amountHeal)
    // {
    //     if (state != BattleStateEnum.PLAYERTURN)
    //         return;
    //     playerUnit.GainHealth(amountHeal);
    //     playerHUD.SetHP(playerUnit.currentHP);

    //     spellBook.SetActive(false);
    //     StartCoroutine(EnemyTurn());
    // }

    private IEnumerator EnemyTurn()
    {
        // TODO:
        // Get current enemy
        // Get enemy moves
        // Pick random attack


        yield return new WaitForSeconds(0.5f);
        // enemyUnit.DealAttack("basicAttack");
        yield return new WaitForSeconds(0.3f);
        // playerUnit.TakeDamage(enemyUnit.damage);
        // State currentState = playerUnit.getState();
        yield return new WaitForSeconds(0.3f);
        // playerHUD.SetHP(playerUnit.currentHP);
        yield return new WaitForSeconds(1f);

        // if (currentState == State.DEAD)
        // {
        //     state = BattleStateEnum.LOST;
        //     StartCoroutine(EndBattle());
        // }
        // else
        // {
        //     state = BattleStateEnum.PLAYERTURN;
        //     PlayerTurn();
        // }
    }

    private IEnumerator EndBattle()
    {
        if (state == BattleStateEnum.WON)
        {
            Debug.Log("You WON");
            if (currentLevel == totalLevels)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
            }
            currentLevel++;
            // enemyUnit.GainFullHealth();
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
}
