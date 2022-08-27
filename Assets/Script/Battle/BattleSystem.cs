using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BattleSystem : MonoBehaviour
{
    /**
     * Prefab references
     */
    public BaseUnit playerUnit;
    public List<BaseUnit> enemyUnits;

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

    /**
     * Current level
     */
    private int currentLevel = 0;

    /**
     * Total levels
     */
    private int totalLevels = 4;

    // Contructor
    void Start()
    {
        spellBook.SetActive(false);
        playerGO = Instantiate(playerUnit.prefab, playerBattleStation);
        Restart();
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
        this.Spawn(this.enemyUnits[currentLevel]);

        playerHUD.SetHUD(this.playerUnit);
        enemyHUD.SetHUD(this.enemyUnits[currentLevel]);

        yield return new WaitForSeconds(1f);

        state = BattleStateEnum.PLAYERTURN;
        PlayerTurn();
    }


    private void PlayerTurn()
    {
        spellBook.SetActive(true);
    }

    private IEnumerator PlayerAttack(Action action)
    {
        // this.playerUnit.Animate(action);
        yield return new WaitForSeconds(0.2f);
        if (action.target == "player")
        {
            Debug.Log(action.actionName + ": PLAYER");
            playerUnit.ResolveAction(action);
        }
        else
            Debug.Log(action.actionName + ": ENEMY");
        {
            enemyUnits[currentLevel].ResolveAction(action);
        }
        yield return new WaitForSeconds(0.2f);
        enemyHUD.SetHP(this.enemyUnits[currentLevel].getHpRemaining());
        playerHUD.SetHP(playerUnit.getHpRemaining());
        yield return new WaitForSeconds(2f);

        if (this.enemyUnits[currentLevel].IsDead())
        {
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

    /**
     * Execute a player action based on action name
     *
     * @param string actionName
     */
    public void PlayerAct(string actionName) {
        if (state != BattleStateEnum.PLAYERTURN) {
            return;
        }

        Action action = this.FindActionOfUnit(this.playerUnit, actionName);
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
    private void Spawn(BaseUnit enemy)
    {
        this.enemyGO = Instantiate(enemy.prefab, enemyBattleStation);
    }


    private IEnumerator EnemyTurn()
    {
        // TODO:
        // Get current enemy
        // Get enemy moves
        // Pick random attack

        List<Action> chooseFrom = this.enemyUnits[currentLevel].actions;
        Action action = this.FindActionOfUnit(this.enemyUnits[currentLevel], this.enemyUnits[currentLevel].actions[RandomHelper.Range(0, chooseFrom.Count)].actionName );
        yield return new WaitForSeconds(0.5f);
        // ANIMATION
        // enemyUnit.DealAttack("basicAttack");
        yield return new WaitForSeconds(0.3f);
        if (action.target == "player") {
            playerUnit.ResolveAction(action);
        } else {
            enemyUnits[currentLevel].ResolveAction(action);
        }
        // State currentState = playerUnit.getState();
        yield return new WaitForSeconds(0.3f);
        enemyHUD.SetHP(this.enemyUnits[currentLevel].getHpRemaining());
        playerHUD.SetHP(playerUnit.getHpRemaining());
        yield return new WaitForSeconds(1f);

        if (playerUnit.IsDead())
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
