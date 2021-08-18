using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;


//Controller for Battle system.
public class BattleController : MonoBehaviour
{
    private BasicMapInfo mapInfo;

    public PHASE_LIST currentPhase;
    public BattleState battleState;

    private List<CharacterInfo> enemies;
    private List<CharacterInfo> playerCharacters;
    private bool[] playerHasMoved;
    private bool[] enemyHasMoved;

    private CharacterInfo selectedUnit;
    private CharacterInfo targetedUnit;

    public PlayerController playerCursor;
    private ActionsMenuController actionMenu;
    private BattleAnimationController animationController;

    [SerializeField]private Stack<BattleState> stateQueue;

    private TileInfo tileToMoveTo;

    private bool processNextEnemy;

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        stateQueue = new Stack<BattleState>();

        playerCharacters = new List<CharacterInfo>();
        playerCursor = GameObject.Find("Player").GetComponent<PlayerController>();

        actionMenu = WorldStateInfo.Instance.actionMenu.GetComponent<ActionsMenuController>();

        animationController = GetComponent<BattleAnimationController>();

        mapInfo = WorldStateInfo.Instance.currentMapInfo;
        currentPhase = PHASE_LIST.PLAYER_PHASE;
        battleState = BattleState.START_STATE;

        enemies = mapInfo.enemies;
        playerCharacters = (mapInfo.playerTeam);

        playerHasMoved = new bool[playerCharacters.Count];
        enemyHasMoved = new bool[enemies.Count];

        for(int i = 0; i < enemyHasMoved.Length; i++)
        {
            enemyHasMoved[i] = false;
            if(i < playerHasMoved.Length)
            {
                playerHasMoved[i] = false;
            }
        }

        actionMenu.DisableUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //State machine to control battle flow. Should be called at the end of any potential action with the state to change to
    //to Queue indicates whether the state needs to be added to the stateQueue. A state only needs to be added to the queue if it is a non processing state
    //so that we can reverse through the states when the user hits the back button
    private void BattleStateMachine(BattleState toState, bool toQueue = true)
    {
        if(toQueue)
            stateQueue.Push(battleState);
        battleState = toState;


        switch (battleState)
        {
            case BattleState.START_STATE:
                break;
            case BattleState.SELECTING_UNIT:
                if(selectedUnit == null || (selectedUnit.characterData.characterType == CHARACTER_TYPE.PLAYER && playerHasMoved[playerCharacters.IndexOf(selectedUnit)]))
                {
                    BattleStateMachine(BattleState.SELECT_ACTION, false);                 
                }
                else if(selectedUnit.characterData.characterType == CHARACTER_TYPE.PLAYER)
                {
                    ColorTiles(selectedUnit.GetGraph());
                }
                else
                {
                    ColorTilesEnemy(selectedUnit.GetGraph());
                }
                break;
            case BattleState.MOVE_UNIT:                
                break;
            case BattleState.VIEWING_ENEMY:
                break;
            case BattleState.SELECT_ACTION:
                BuildActionMenu();
                break;
            case BattleState.ATTACK_AND_MOVE:
                AttackAndMoveAction();
                break;
            case BattleState.CHANGE_PHASE:
                StartEnemyPhase();
                break;
            case BattleState.SECOND_ATTACK:
                break;
            case BattleState.POST_COMBAT:
                PostBattleCleanup();
                break;
            case BattleState.ANIMATING:

                break;
            case BattleState.CLEANUP:
                PostActionCleanup();
                break;
            default:
                stateQueue.Clear();
                BattleStateMachine(BattleState.START_STATE, false);
                break;
        }
    }


    public void ReverseBattleState()
    {
        switch (battleState)
        {
            case BattleState.MOVE_UNIT:
                CancelColorTiles();
                break;
            case BattleState.VIEWING_ENEMY:
                CancelColorTiles();
                break;
            case BattleState.SELECT_ACTION:
                actionMenu.ReverseMenu();
                break;
        }
    }

    private void StartPlayerTurn()
    {
        currentPhase = PHASE_LIST.PLAYER_PHASE;

        for(int i = 0; i < playerCharacters.Count; i++)
        {
            if (playerCharacters[i].flagIsDead)
                playerHasMoved[i] = true;
            else
                playerHasMoved[i] = false;
        }

        BattleStateMachine(BattleState.START_STATE);
    }

    private IEnumerator HoldAction(BattleState toState, bool toQueue = true)
    {
        yield return new WaitUntil(() => animationController.isAnimating != true);

        BattleStateMachine(toState, toQueue);
    }

    //Handles how game behaves when you need to select a tile, whether that is to select a unit on the map or to move a previously selected unit
    public void SelectTile()
    {
        //Set selected unit to occupant of the tile selected then call BattleStateMachine to process what state comes next. Selected unit may be null
        if (battleState == BattleState.DEFAULT || battleState == BattleState.START_STATE || battleState == BattleState.VIEWING_ENEMY)
        {
            //RaycastHit2D hit = Physics2D.Raycast(playerCursor.transform.position, new Vector2(1, 0), .1f, LayerMask.GetMask("CharacterLayer"));

            selectedUnit = playerCursor.currentTile.GetOccupant();                  

            if (GameObject.Find("CurrentSelectableTiles") != null)
            {
                UnityEngine.Object.Destroy(GameObject.Find("CurrentSelectableTiles"));
            }
            //if (hit.collider != null)
            //{
            //    selectedUnit = hit.collider.gameObject.GetComponent<CharacterInfo>();
            //    selectedUnit.GetTargetList();
            //    switch (selectedUnit.characterData.characterType)
            //    {
            //        case CHARACTER_TYPE.PLAYER:
            //            if (!playerHasMoved[playerCharacters.FindIndex(x => x == selectedUnit)])
            //            {
            //                ColorTiles(selectedUnit.GetGraph());
            //                battleState = BattleState.MOVE_UNIT;
            //            }
            //            break;
            //        case CHARACTER_TYPE.ENEMY:
            //            ColorTilesEnemy(selectedUnit.GetGraph());
            //            break;
            //        default:
            //            break;
            //    }    
            //}
            BattleStateMachine(BattleState.SELECTING_UNIT);
            
            return;
        }
        //If a moveable unit is selected, handle how to process vaild moves for unit
        if (battleState == BattleState.MOVE_UNIT)
        {
            RaycastHit2D hit = Physics2D.Raycast(playerCursor.transform.position, new Vector2(1, 0), .1f, LayerMask.GetMask("SelectableTile"));
            
            if (hit.collider != null)
            {
                hit = Physics2D.Raycast(playerCursor.transform.position, new Vector2(1, 0), .1f, LayerMask.GetMask("Tile"));
                Node moveableNode = selectedUnit.moveableTiles.Find(x => x.tile == hit.collider.gameObject.GetComponent<TileInfo>());
                if (moveableNode.colorType == COLOR_TYPE.MOVEMENT && (!moveableNode.tile.isOccupied || moveableNode.tile.occupant == selectedUnit))
                {
                    tileToMoveTo = moveableNode.tile;
                    //selectedUnit.MoveCharacter(tileToMoveTo);
                    animationController.MoveCharacterToPosition(selectedUnit, tileToMoveTo);
                    UnityEngine.Object.Destroy(GameObject.Find("CurrentSelectableTiles"));

                    selectedUnit.GetLocalTargets(playerCursor.currentTile);

                    StartCoroutine(HoldAction(BattleState.SELECT_ACTION, false));
                    
                    //BattleStateMachine(BattleState.SELECT_ACTION, false);

                }
                else if (moveableNode.tile.isOccupied && (moveableNode.colorType == COLOR_TYPE.ATTACK || (moveableNode.colorType == COLOR_TYPE.MOVEMENT && moveableNode.tile.occupant.characterData.characterType == CHARACTER_TYPE.ENEMY)))
                {
                    //int closestTile = FindClosestTile(hit.collider.gameObject.GetComponent<TileInfo>());
                    //tileToMoveTo = selectedUnit.FindBestTile(hit.collider.gameObject.GetComponent<TileInfo>().occupant);
                    //animationController.MoveCharacterToPosition(selectedUnit, tileToMoveTo);

                    //UnityEngine.Object.Destroy(GameObject.Find("CurrentSelectableTiles"));

                    //selectedUnit.GetLocalTargets(playerCursor.currentTile);


                    BattleStateMachine(BattleState.ATTACK_AND_MOVE, false);
                }
                
            }
        }
    }

    //Colors the tiles of a selected unit based off of its team and what its color in the graph was set as
    public void ColorTiles(List<Node> moveableTiles)
    {
        List<TileInfo> movementTiles = new List<TileInfo>();

        GameObject currentSelectableTiles = new GameObject("CurrentSelectableTiles");
        string objectName = "Selectable";
        int index = 0;
        foreach (Node tile in moveableTiles)
        {
            GameObject moveableTileSprite;
            moveableTileSprite = new GameObject(objectName + index);
            moveableTileSprite.layer = 9;

            movementTiles.Add(tile.tile);

            SpriteRenderer renderer = moveableTileSprite.AddComponent<SpriteRenderer>();
            switch (tile.colorType)
            {
                case COLOR_TYPE.MOVEMENT:
                    renderer.sprite = Resources.Load<Sprite>("Sprites/TileMaps/GlobalTiles/MoveableTile") as Sprite;
                    break;
                case COLOR_TYPE.ATTACK:
                    renderer.sprite = Resources.Load<Sprite>("Sprites/TileMaps/GlobalTiles/RangeTile") as Sprite;
                    break;
                default:
                    break;

            }
            moveableTileSprite.transform.parent = currentSelectableTiles.transform;

            BoxCollider2D collider = moveableTileSprite.AddComponent<BoxCollider2D>();

            moveableTileSprite.transform.position = tile.tile.transform.position;
            index++;
        }
        BattleStateMachine(BattleState.MOVE_UNIT);
    }
    //All enemy tiles indicating movement and range are colored red
    private void ColorTilesEnemy(List<Node> moveableTiles)
    {
        List<TileInfo> movementTiles = new List<TileInfo>();

        GameObject currentSelectableTiles = new GameObject("CurrentSelectableTiles");
        string objectName = "Selectable";
        int index = 0;
        foreach (Node tile in moveableTiles)
        {
            GameObject moveableTileSprite;
            moveableTileSprite = new GameObject(objectName + index);
            moveableTileSprite.layer = 9;

            movementTiles.Add(tile.tile);

            SpriteRenderer renderer = moveableTileSprite.AddComponent<SpriteRenderer>();
            
            renderer.sprite = Resources.Load<Sprite>("Sprites/TileMaps/GlobalTiles/RangeTile") as Sprite;
                    
            moveableTileSprite.transform.parent = currentSelectableTiles.transform;

            BoxCollider2D collider = moveableTileSprite.AddComponent<BoxCollider2D>();

            moveableTileSprite.transform.position = tile.tile.transform.position;
            index++;
        }

        BattleStateMachine(BattleState.VIEWING_ENEMY);
    }

    private void CancelColorTiles()
    {
        UnityEngine.Object.Destroy(GameObject.Find("CurrentSelectableTiles"));
        selectedUnit.MoveCharacter(selectedUnit.currentTile);
        playerCursor.MoveCursor(selectedUnit.transform.position, 5.0f);

        playerCursor.currentTile = selectedUnit.currentTile;

        selectedUnit = null;

        stateQueue.Clear();

        BattleStateMachine(BattleState.START_STATE, false);
    }

    //private List<GameObject> AddRangeTiles(Node tile)
    //{
    //    List<GameObject> rangeTiles = new List<GameObject>();
    //    foreach (GameObject rangeTile in tile.tilesInRange)
    //    {
    //        rangeTiles.Add(rangeTile);
    //    }

    //    return rangeTiles;
    //}

    //If an enemy unit is selected while in BattleState.MOVE_UNIT, use dijkstras to find the closest tile for unit to move to
    //private int FindClosestTile(TileInfo target)
    //{
    //List<Node> graph = selectedUnit.GetGraph();
    //COLOR_TYPE currentTileColor;
    //int targetIndex = graph.Find(x => x.tile == target).nodeIndex;
    //int currentTile = targetIndex;
    //int currentRange = 0;

    //do
    //{
    //    currentTile = selectedUnit.shortestPath.parent[currentTile];
    //    currentTileColor = graph.Find(x => x.nodeIndex == currentTile).colorType;
    //    currentRange = currentRange + 1;
    //} while (currentTileColor != COLOR_TYPE.MOVEMENT || (graph.Find(x=>x.nodeIndex == currentTile).tile.isOccupied && graph.Find(x => x.nodeIndex == currentTile).tile.occupant != selectedUnit) || currentRange != selectedUnit.GetMaxRange());


    //return currentTile;
    //}

    //Builds the action menu when needed. See ActionsMenuController for more detail
    private void BuildActionMenu()
    {
        if(selectedUnit == null)
        {
            DefaultActionMenu();
        }
        else
        {
            CharacterActionMenu();
        }
        playerCursor.SwitchControlType("UI");
    }

    private void DefaultActionMenu()
    {
        List<ACTION_BUTTON_LIST> actions = new List<ACTION_BUTTON_LIST>();
        actions.Add(ACTION_BUTTON_LIST.UNITS);
        actions.Add(ACTION_BUTTON_LIST.OBJECTIVE);
        actions.Add(ACTION_BUTTON_LIST.OPTIONS);
        actions.Add(ACTION_BUTTON_LIST.SUSPEND);
        actions.Add(ACTION_BUTTON_LIST.END_TURN);

        actionMenu.BuildActionMenu(actions);

        actionMenu.EnableUI();
    }

    private void CharacterActionMenu()
    {
        List<ACTION_BUTTON_LIST> actions = new List<ACTION_BUTTON_LIST>();
        if(selectedUnit.localTargets.Count > 0)
        {
            actions.Add(ACTION_BUTTON_LIST.ATTACK);
        }
        actions.Add(ACTION_BUTTON_LIST.INVENTORY);
        actions.Add(ACTION_BUTTON_LIST.WAIT);

        actionMenu.BuildActionMenu(actions);

        actionMenu.EnableUI();
    }

    public void CancelActionMenu()
    {
        actionMenu.ResetMenus();
        playerCursor.SwitchControlType("FieldBattle");
        playerCursor.MoveCursor(playerCursor.currentTile.transform.position, 1.0f);
        if(selectedUnit != null)
            selectedUnit.MoveCharacter(selectedUnit.currentTile);

        BattleStateMachine(stateQueue.Pop(), false);
    }

    #region ATTACK_ACTION

    public void StartAttackAction()
    {
        
    }

    public void AttackAction()
    {
        actionMenu.CreateItemPanel(selectedUnit.weaponList, selectedUnit.itemList, ACTION_BUTTON_LIST.ATTACK);        
    }

    public void SelectTarget(WeaponData selectedWeapon)
    {
        actionMenu.CreateTargetList(selectedUnit.localTargets, selectedWeapon);
    }

    public void AttackAndMoveAction()
    {
        actionMenu.AttackAndMovePanel(selectedUnit.weaponList, playerCursor.currentTile.occupant);
        actionMenu.EnableUI();
        playerCursor.SwitchControlType("UI");
    }

    public void MoveToAttackPosition(WeaponData weapon, CharacterInfo target)
    {
        tileToMoveTo = selectedUnit.FindBestTile(weapon, target);

        animationController.MoveCharacterToPosition(selectedUnit, tileToMoveTo);
        UnityEngine.Object.Destroy(GameObject.Find("CurrentSelectableTiles"));

        StartCoroutine(WaitForMove(weapon, target));
    }

    public IEnumerator WaitForMove(WeaponData weapon, CharacterInfo target)
    {
        yield return new WaitUntil(() => !animationController.isAnimating);

        ProcessAttack(target, weapon);
    }

    public void ProcessAttack(CharacterInfo selectedTarget, WeaponData selectedWeapon)
    {
        targetedUnit = selectedTarget;

        int crit = 1;
        int playerHit,  playerCrit;
        int enemyDodge;

        playerHit = selectedUnit.characterData.HIT + selectedWeapon.HIT;
        playerCrit = selectedUnit.characterData.CRIT + selectedWeapon.CRIT;

        enemyDodge = selectedTarget.characterData.DODGE + selectedTarget.currentTile.dodgeBoost;

        if (DoesAttackHit(playerHit, enemyDodge))
        {
            if (WorldStateInfo.Instance.GetNextRandomNumber() < playerCrit)
                crit = 3;
            if (selectedWeapon.isRanged)
            {
                ProcessDamageRanged(selectedUnit, selectedTarget, selectedWeapon, crit);
            }
            else
            {
                ProcessDamageMelee(selectedUnit, selectedTarget, selectedWeapon, crit);
            }
            PostDamageProcessing(selectedTarget);

        }

        animationController.AddToAnimationQueue(selectedUnit, selectedTarget);

        if (CanCounterAttack(selectedUnit, selectedTarget) && !selectedTarget.flagIsDead)
        {
            ProcessEnemyAttack(selectedTarget, selectedUnit);
            PostDamageProcessing(selectedUnit);
            animationController.AddToAnimationQueue(selectedTarget, selectedUnit);
        }

        CharacterInfo secondAttack = ProcessSecondAttack(selectedUnit, selectedTarget);

        if(secondAttack != null && !secondAttack.flagIsDead)
        {
            if (secondAttack == selectedUnit)
            {
                if (WorldStateInfo.Instance.GetNextRandomNumber() < playerCrit)
                    crit = 3;
                if (selectedWeapon.isRanged)
                {
                    ProcessDamageRanged(selectedUnit, selectedTarget, selectedWeapon, crit);
                }
                else
                {
                    ProcessDamageMelee(selectedUnit, selectedTarget, selectedWeapon, crit);
                }
                PostDamageProcessing(selectedTarget);
                animationController.AddToAnimationQueue(selectedUnit, selectedTarget);
            }
            else if(secondAttack == selectedTarget)
            {
                ProcessEnemyAttack(selectedTarget, selectedUnit);
                PostDamageProcessing(selectedUnit);
                animationController.AddToAnimationQueue(selectedTarget, selectedUnit);
            }
            
        }

        //if (selectedUnit.GetHPCurrent() <= 0)
        //{
        //    selectedUnit.flagIsDead = true;
        //    selectedUnit.gameObject.SetActive(false);
        //}
        //if (selectedTarget.GetHPCurrent() <= 0)
        //{
        //    selectedTarget.flagIsDead = true;
        //    selectedTarget.gameObject.SetActive(false);
        //}

        animationController.AttackAnimationProcessing();

        StartCoroutine(HoldAction(BattleState.POST_COMBAT));
        
        //BattleStateMachine(BattleState.POST_COMBAT);
    }

    private bool DoesAttackHit(int attackerHit, int defenderDodge)
    {
        int hitRoll = WorldStateInfo.Instance.GetNextRandomNumber();

        if (hitRoll < attackerHit - defenderDodge)
        {
            return true;
        }
        else
            return false;
    }

    private void ProcessDamageRanged(CharacterInfo attackingUnit, CharacterInfo selectedTarget, WeaponData selectedWeapon, int crit)
    {
        switch (selectedWeapon.damageType)
        {
            case DAMAGE_TYPE.ENERGY:
                selectedTarget.characterData.HP_CURRENT = selectedTarget.characterData.HP_CURRENT - ((attackingUnit.characterData.PREC + selectedWeapon.ATK * (crit)) - selectedTarget.characterData.SHIELD);
                break;
            case DAMAGE_TYPE.PHYSICAL:
                selectedTarget.characterData.HP_CURRENT = selectedTarget.characterData.HP_CURRENT - ((attackingUnit.characterData.PREC + selectedWeapon.ATK * (crit)) - selectedTarget.characterData.ARMOR);
                break;
            default:
                selectedTarget.characterData.HP_CURRENT = selectedTarget.characterData.HP_CURRENT - ((attackingUnit.characterData.PREC + selectedWeapon.ATK * (crit)) - selectedTarget.characterData.ARMOR);
                break;
        }
    }

    private void ProcessDamageMelee(CharacterInfo attackingUnit, CharacterInfo selectedTarget, WeaponData selectedWeapon, int crit)
    {
        switch (selectedWeapon.damageType)
        {
            case DAMAGE_TYPE.ENERGY:
                selectedTarget.characterData.HP_CURRENT = selectedTarget.characterData.HP_CURRENT - ((attackingUnit.characterData.PHY + selectedWeapon.ATK * (crit)) - selectedTarget.characterData.SHIELD);
                break;
            case DAMAGE_TYPE.PHYSICAL:
                selectedTarget.characterData.HP_CURRENT = selectedTarget.characterData.HP_CURRENT - ((attackingUnit.characterData.PHY + selectedWeapon.ATK * (crit)) - selectedTarget.characterData.ARMOR);
                break;
            default:
                selectedTarget.characterData.HP_CURRENT = selectedTarget.characterData.HP_CURRENT - ((attackingUnit.characterData.PHY + selectedWeapon.ATK * (crit)) - selectedTarget.characterData.ARMOR);
                break;
        }
    }

    private bool CanCounterAttack(CharacterInfo attackingUnit, CharacterInfo defendingUnit)
    {
        defendingUnit.GetLocalTargetsCurrentWeapon();
        if(defendingUnit.localTargets.Exists(x=> x == attackingUnit))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private CharacterInfo ProcessSecondAttack(CharacterInfo attacker, CharacterInfo defender)
    {
        CharacterInfo secondAttack = null;
        if (attacker.characterData.SPEED - defender.characterData.SPEED >= 5)
            secondAttack = attacker;
        if (defender.characterData.SPEED - attacker.characterData.SPEED >= 5)
            secondAttack = defender;

        return secondAttack;
    }

    private void PostDamageProcessing(CharacterInfo defender)
    {
        if(defender.characterData.HP_CURRENT <= 0)
        {
            defender.flagIsDead = true;
        }
    }

    private void PostBattleCleanup()
    {
        //actionMenu.ResetMenus();
        //playerCursor.SwitchControlType("FieldBattle");
        //playerCursor.MoveCursor(playerCursor.currentTile.transform.position, 1.0f);

        //playerHasMoved[playerCharacters.IndexOf(selectedUnit)] = true;
        //selectedUnit.SetNewTile(playerCursor.currentTile);
        //selectedUnit = null;

        //BattleStateMachine(BattleState.START_STATE);

        if (targetedUnit.flagIsDead)
        {
            targetedUnit.gameObject.SetActive(false);
        }
        if (selectedUnit.flagIsDead)
        {
            selectedUnit.gameObject.SetActive(false);
        }
        PostActionCleanup();
    }

    #endregion

    public void InventoryAction()
    {
        actionMenu.CreateItemPanel(selectedUnit.weaponList, selectedUnit.itemList, ACTION_BUTTON_LIST.INVENTORY);
    }

    public void EquipWeapon(WeaponData weapon)
    {
        selectedUnit.ChangeWeapon(weapon);
        BattleStateMachine(BattleState.CLEANUP);
    }

    public void WaitAction()
    {
        PostActionCleanup();
    }

    private void PostActionCleanup()
    {
        actionMenu.ResetMenus();
        playerCursor.SwitchControlType("FieldBattle");
        playerCursor.MoveCursor(playerCursor.currentTile.transform.position, 1.0f);

        playerHasMoved[playerCharacters.IndexOf(selectedUnit)] = true;
        selectedUnit.SetNewTile(tileToMoveTo);
        selectedUnit = null;

        foreach(var character in playerCharacters)
        {
            character.RecolorGraph();
        }

        if (ToEnemyPhase())
        {
            currentPhase = PHASE_LIST.ENEMY_PHASE;
            BattleStateMachine(BattleState.CHANGE_PHASE);
        }
        else
            BattleStateMachine(BattleState.START_STATE);
    }

    private bool ToEnemyPhase()
    {
        bool phaseChange = true;

        for(int i = 0; i < playerHasMoved.Length; i++)
        {
            if (playerHasMoved[i] == false)
                phaseChange = false;
        }

        return phaseChange;
    }






    #region ENEMY_PHASE
    private void StartEnemyPhase()
    {
        processNextEnemy = true;
        for(int i = 0; i < enemyHasMoved.Length; i++)
        {
            enemyHasMoved[i] = false;
        }
        StartCoroutine(PerformEnemyPhase());
    }

    private IEnumerator PerformEnemyPhase()
    {
        foreach(var enemy in enemies)
        {            
            if (!enemy.flagIsDead)
            {
                processNextEnemy = false;
                enemy.enemyAI.GetTarget();

                if (enemy.enemyAI.chosenAction == ENEMY_ACTIONS.ATTACK)
                {
                    FindClosestTileAndAttack(enemy);
                }
                else if (enemy.enemyAI.chosenAction == ENEMY_ACTIONS.MOVE)
                {

                }
            }
            enemyHasMoved[enemies.IndexOf(enemy)] = true;
            yield return new WaitUntil(() => processNextEnemy);
        }

        StartPlayerTurn();
    }

    private void FindClosestTileAndAttack(CharacterInfo enemy)
    {
        enemy.SetNewTile(enemy.currentTile);
        enemy.RunDijsktras();

        //List<Node> graph = enemy.GetGraph();
        //COLOR_TYPE currentTileColor;
        //int targetIndex = graph.Find(x => x.tile == enemy.enemyAI.chosenTarget.currentTile).nodeIndex;
        //int currentTile = targetIndex;
        //int currentRange = 0;
        ////if (enemy.weaponList[enemy.enemyAI.chosenWeapon].MAXRANGE)
        //do
        //{
        //    currentTile = enemy.shortestPath.parent[currentTile];
        //    currentTileColor = graph.Find(x => x.nodeIndex == currentTile).colorType;
        //    currentRange = currentRange + 1;
        //} while (currentTileColor != COLOR_TYPE.MOVEMENT || (graph.Find(x => x.nodeIndex == currentTile).tile.isOccupied && graph.Find(x => x.nodeIndex == currentTile).tile.occupant != enemy) || currentRange != enemy.weaponList[enemy.enemyAI.chosenWeapon].MAXRANGE);

        animationController.MoveCharacterToPosition(enemy, enemy.subGraph.graphNodes.Find(x=>x.tile == enemy.enemyAI.chosenTile).tile);

        //enemy.MoveCharacter(enemy.moveableTiles.Find(x => x.nodeIndex == currentTile).tile);

        StartCoroutine(HoldEnemyAction(ProcessEnemyAttack, enemy));

        enemy.SetNewTile(enemy.moveableTiles.Find(x => x.tile == enemy.enemyAI.chosenTile).tile);

        //ProcessEnemyAttack(enemy);
    }

    private void ProcessEnemyAttack(CharacterInfo enemy)
    {
        WeaponData weapon = enemy.weaponList[enemy.enemyAI.chosenWeapon];
        CharacterInfo target = enemy.enemyAI.chosenTarget;

        int playerHit, playerDodge, playerCrit;
        int enemyHit, enemyDodge, enemyCrit;

        playerHit = target.characterData.HIT + target.weaponList[0].HIT;
        playerDodge = target.characterData.DODGE + target.currentTile.dodgeBoost;
        playerCrit = target.characterData.CRIT + target.weaponList[0].CRIT;

        enemyHit = enemy.characterData.HIT + weapon.HIT;
        enemyDodge = enemy.characterData.DODGE + enemy.currentTile.dodgeBoost;
        enemyCrit = enemy.characterData.CRIT + weapon.HIT;

        int crit = 1;

        if (DoesAttackHit(enemyHit, playerDodge))
        {
            if (WorldStateInfo.Instance.GetNextRandomNumber() < enemyCrit)
                crit = 3;
            if (weapon.isRanged)
            {
                switch (weapon.damageType)
                {
                    case DAMAGE_TYPE.ENERGY:
                        target.characterData.HP_CURRENT = target.characterData.HP_CURRENT - ((enemy.characterData.PREC + weapon.ATK * (crit)) - target.characterData.SHIELD);
                        break;
                    case DAMAGE_TYPE.PHYSICAL:
                        target.characterData.HP_CURRENT = target.characterData.HP_CURRENT - ((enemy.characterData.PREC + weapon.ATK * (crit)) - target.characterData.ARMOR);
                        break;
                    default:
                        target.characterData.HP_CURRENT = target.characterData.HP_CURRENT - ((enemy.characterData.PREC + weapon.ATK * (crit)) - target.characterData.ARMOR);
                        break;
                }
            }
            else
            {
                switch (weapon.damageType)
                {
                    case DAMAGE_TYPE.ENERGY:
                        target.characterData.HP_CURRENT = target.characterData.HP_CURRENT - ((enemy.characterData.PHY + weapon.ATK * (crit)) - target.characterData.SHIELD);
                        break;
                    case DAMAGE_TYPE.PHYSICAL:
                        target.characterData.HP_CURRENT = target.characterData.HP_CURRENT - ((enemy.characterData.PHY + weapon.ATK * (crit)) - target.characterData.ARMOR);
                        break;
                    default:
                        target.characterData.HP_CURRENT = target.characterData.HP_CURRENT - ((enemy.characterData.PHY + weapon.ATK * (crit)) - target.characterData.ARMOR);
                        break;
                }
            }
            PostDamageProcessing(target);
        }
        animationController.AddToAnimationQueue(enemy, target);

        if(CanCounterAttack(enemy, target))
        {
            if (DoesAttackHit(playerHit, enemyDodge))
            {
                if (WorldStateInfo.Instance.GetNextRandomNumber() < playerCrit)
                    crit = 3;
                if (target.weaponList[0].isRanged)
                {
                    ProcessDamageRanged(target, enemy, target.weaponList[0], crit);
                }
                else
                {
                    ProcessDamageMelee(target, enemy, target.weaponList[0], crit);
                }
                PostDamageProcessing(enemy);

            }
            animationController.AddToAnimationQueue(target, enemy);
        }

        CharacterInfo secondAttack = ProcessSecondAttack(enemy, target);

        if (secondAttack != null && !secondAttack.flagIsDead)
        {
            if (secondAttack == target)
            {
                if (WorldStateInfo.Instance.GetNextRandomNumber() < playerCrit)
                    crit = 3;
                if (target.weaponList[0].isRanged)
                {
                    ProcessDamageRanged(target, enemy, target.weaponList[0], crit);
                }
                else
                {
                    ProcessDamageMelee(target, enemy, target.weaponList[0], crit);
                }
                PostDamageProcessing(enemy);
                animationController.AddToAnimationQueue(target, enemy);
            }
            else if (secondAttack == enemy)
            {
                ProcessEnemyAttack(enemy, target);
                PostDamageProcessing(selectedUnit);
                animationController.AddToAnimationQueue(enemy, target);
            }
            
        }

        animationController.AttackAnimationProcessing();

        StartCoroutine(HoldEnemyAction(EnemyCleanup, enemy));
        
    }

    private void ProcessEnemyAttack(CharacterInfo enemy, CharacterInfo target)
    {
        WeaponData weapon = enemy.weaponList[enemy.enemyAI.chosenWeapon];

        int playerDodge;
        int enemyHit, enemyCrit;

        playerDodge = enemy.characterData.DODGE + selectedUnit.currentTile.dodgeBoost;

        enemyHit = enemy.characterData.HIT + weapon.HIT;
        enemyCrit = enemy.characterData.CRIT + weapon.HIT;

        int crit = 1;

        if (DoesAttackHit(enemyHit, playerDodge))
        {
            if (WorldStateInfo.Instance.GetNextRandomNumber() < enemyCrit)
                crit = 3;
            if (weapon.isRanged)
            {
                switch (weapon.damageType)
                {
                    case DAMAGE_TYPE.ENERGY:
                        target.characterData.HP_CURRENT = target.characterData.HP_CURRENT - ((enemy.characterData.PREC + weapon.ATK * (crit)) - target.characterData.SHIELD);
                        break;
                    case DAMAGE_TYPE.PHYSICAL:
                        target.characterData.HP_CURRENT = target.characterData.HP_CURRENT - ((enemy.characterData.PREC + weapon.ATK * (crit)) - target.characterData.ARMOR);
                        break;
                    default:
                        target.characterData.HP_CURRENT = target.characterData.HP_CURRENT - ((enemy.characterData.PREC + weapon.ATK * (crit)) - target.characterData.ARMOR);
                        break;
                }
            }
            else
            {
                switch (weapon.damageType)
                {
                    case DAMAGE_TYPE.ENERGY:
                        target.characterData.HP_CURRENT = target.characterData.HP_CURRENT - ((enemy.characterData.PHY + weapon.ATK * (crit)) - target.characterData.SHIELD);
                        break;
                    case DAMAGE_TYPE.PHYSICAL:
                        target.characterData.HP_CURRENT = target.characterData.HP_CURRENT - ((enemy.characterData.PHY + weapon.ATK * (crit)) - target.characterData.ARMOR);
                        break;
                    default:
                        target.characterData.HP_CURRENT = target.characterData.HP_CURRENT - ((enemy.characterData.PHY + weapon.ATK * (crit)) - target.characterData.ARMOR);
                        break;
                }
            }
        }
    }

    private void EnemyCleanup(CharacterInfo enemy)
    {
        if (enemy.flagIsDead == true)
            enemy.gameObject.SetActive(false);
        if (enemy.enemyAI.chosenTarget.flagIsDead == true)
            enemy.enemyAI.chosenTarget.gameObject.SetActive(false);

        processNextEnemy = true;
    }

    private IEnumerator HoldEnemyAction(Action<CharacterInfo> nextAction, CharacterInfo enemy)
    {
        yield return new WaitUntil(() => animationController.isAnimating == false);

        nextAction(enemy);
    }

    private IEnumerator HoldEnemyAction(Action nextAction)
    {
        yield return new WaitUntil(() => animationController.isAnimating == false);
    }

    #endregion






    public List<CharacterInfo> GetPlayerUnits()
    {
        return playerCharacters;
    }
}
