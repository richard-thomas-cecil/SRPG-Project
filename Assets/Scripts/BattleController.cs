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
    private UnitInfoController unitInfoController;
    private BattlePreviewController battlePreviewController;
    private TileInfoController tileInfoController;
    private WeaponInfoController weaponInfoController;
    private HealthPanelController healthPanelController;
    private UnitWindowController unitWindow;
    private UnitDetailsWindowController unitDetailsWindow;
    private ResultsScreenController resultsScreen;

    [SerializeField]private Stack<BattleState> stateQueue;

    private TileInfo tileToMoveTo;

    private bool processNextEnemy;

    void Awake()
    {
        stateQueue = new Stack<BattleState>();

        playerCharacters = new List<CharacterInfo>();
        playerCursor = GameObject.Find("Player").GetComponent<PlayerController>();

        
    }

    // Start is called before the first frame update
    void Start()
    {
        //actionMenu = WorldStateInfo.Instance.actionMenu.GetComponent<ActionsMenuController>();
        //unitInfoController = WorldStateInfo.Instance.unitInfoPanel.GetComponent<UnitInfoController>();
        //battlePreviewController = WorldStateInfo.Instance.battlePreview.GetComponent<BattlePreviewController>();
        //tileInfoController = WorldStateInfo.Instance.tileInfoPanel.GetComponent<TileInfoController>();
        //weaponInfoController = WorldStateInfo.Instance.weaponInfoPanel.GetComponent<WeaponInfoController>();
        //healthPanelController = WorldStateInfo.Instance.healthPanel.GetComponent<HealthPanelController>();
        //unitWindow = WorldStateInfo.Instance.unitWindow.GetComponent<UnitWindowController>();
        //unitDetailsWindow = WorldStateInfo.Instance.unitDetailsWindow.GetComponent<UnitDetailsWindowController>();
        //resultsScreen = WorldStateInfo.Instance.resultScreen.GetComponent<ResultsScreenController>();

        //animationController = GetComponent<BattleAnimationController>();

        //mapInfo = WorldStateInfo.Instance.currentMapInfo;
        //currentPhase = PHASE_LIST.PLAYER_PHASE;
        //battleState = BattleState.START_STATE;

        //enemies = mapInfo.enemies;
        //playerCharacters = (mapInfo.playerTeam);

        //playerHasMoved = new bool[playerCharacters.Count];
        //enemyHasMoved = new bool[enemies.Count];

        //unitWindow.BuildWindow(playerCharacters);

        //for(int i = 0; i < enemyHasMoved.Length; i++)
        //{
        //    enemyHasMoved[i] = false;
        //    if(i < playerHasMoved.Length)
        //    {
        //        playerHasMoved[i] = false;
        //    }
        //}

        //actionMenu.InitialzeButtonFunctions();

        //actionMenu.DisableUI();
        //unitWindow.DisableWindow();
        //unitDetailsWindow.DisablePanel();
        //resultsScreen.DisableScreen();
    }

    // Update is called once per frame
    void Update()
    {
        if(tileInfoController.currentTile != playerCursor.currentTile && battleState != BattleState.BATTLE_OVER)
        {
            tileInfoController.SetPanel(playerCursor.currentTile);
            if(playerCursor.currentTile.occupant != null)
                unitInfoController.SetPanel(playerCursor.currentTile.occupant);
            else
            {
                unitInfoController.DisablePanel();
            }
        }

    }

    public void InitializeBattle()
    {
        actionMenu = WorldStateInfo.Instance.actionMenu.GetComponent<ActionsMenuController>();
        unitInfoController = WorldStateInfo.Instance.unitInfoPanel.GetComponent<UnitInfoController>();
        battlePreviewController = WorldStateInfo.Instance.battlePreview.GetComponent<BattlePreviewController>();
        tileInfoController = WorldStateInfo.Instance.tileInfoPanel.GetComponent<TileInfoController>();
        weaponInfoController = WorldStateInfo.Instance.weaponInfoPanel.GetComponent<WeaponInfoController>();
        healthPanelController = WorldStateInfo.Instance.healthPanel.GetComponent<HealthPanelController>();
        unitWindow = WorldStateInfo.Instance.unitWindow.GetComponent<UnitWindowController>();
        unitDetailsWindow = WorldStateInfo.Instance.unitDetailsWindow.GetComponent<UnitDetailsWindowController>();
        resultsScreen = WorldStateInfo.Instance.resultScreen.GetComponent<ResultsScreenController>();

        animationController = GetComponent<BattleAnimationController>();

        mapInfo = WorldStateInfo.Instance.currentMapInfo;
        currentPhase = PHASE_LIST.PLAYER_PHASE;
        battleState = BattleState.START_STATE;

        enemies = mapInfo.enemies;
        playerCharacters = (mapInfo.playerTeam);

        playerHasMoved = new bool[playerCharacters.Count];
        enemyHasMoved = new bool[enemies.Count];

        unitWindow.BuildWindow(playerCharacters);

        for (int i = 0; i < enemyHasMoved.Length; i++)
        {
            enemyHasMoved[i] = false;
            if (i < playerHasMoved.Length)
            {
                playerHasMoved[i] = false;
            }
        }

        actionMenu.InitialzeButtonFunctions();

        actionMenu.DisableUI();
        unitWindow.DisableWindow();
        unitDetailsWindow.DisablePanel();
        resultsScreen.DisableScreen();
    }

    //State machine to control battle flow. Should be called at the end of any potential action with the state to change to
    //to Queue indicates whether the state needs to be added to the stateQueue. A state only needs to be added to the queue if it is a non processing state
    //so that we can reverse through the states when the user hits the back button
    private void BattleStateMachine(BattleState toState, bool toQueue = true)
    {
        if (toState != BattleState.BATTLE_OVER)
        {
            if (toQueue)
                stateQueue.Push(battleState);
            battleState = toState;

        
            switch (battleState)
            {
                case BattleState.START_STATE:
                    break;
                case BattleState.SELECTING_UNIT:
                    if (selectedUnit == null || (selectedUnit.characterData.characterType == CHARACTER_TYPE.PLAYER && playerHasMoved[playerCharacters.IndexOf(selectedUnit)]))
                    {
                        BattleStateMachine(BattleState.SELECT_ACTION, false);
                    }
                    else if (selectedUnit.characterData.characterType == CHARACTER_TYPE.PLAYER)
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
                    currentPhase = PHASE_LIST.ENEMY_PHASE;
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
                case BattleState.UNIT_DETAILS:
                    break;
                case BattleState.BATTLE_OVER:
                    break;
                default:
                    stateQueue.Clear();
                    BattleStateMachine(BattleState.START_STATE, false);
                    break;
            }
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
                if(actionMenu.gameObject.activeSelf)
                    actionMenu.ReverseMenu();
                if (unitWindow.gameObject.activeSelf)
                    DisableUnitWindow();
                break;
            case BattleState.UNIT_DETAILS:
                OpenUnitDetails();
                break;

        }
    }

    private void StartPlayerTurn()
    {
        playerCursor.ToggleControls();
        currentPhase = PHASE_LIST.PLAYER_PHASE;

        for(int i = 0; i < playerCharacters.Count; i++)
        {
            if (playerCharacters[i].flagIsDead)
                playerHasMoved[i] = true;
            else
            {
                playerHasMoved[i] = false;
                playerCharacters[i].SetNewTile(playerCharacters[i].currentTile);
            }
        }

        BattleStateMachine(BattleState.START_STATE);
    }

    private IEnumerator HoldAction(BattleState toState, bool toQueue = true)
    {
        yield return new WaitUntil(() => animationController.isAnimating != true);

        playerCursor.ToggleControls();
        BattleStateMachine(toState, toQueue);
    }

    //Handles how game behaves when you need to select a tile, whether that is to select a unit on the map or to move a previously selected unit
    public void SelectTile()
    {
        //Set selected unit to occupant of the tile selected then call BattleStateMachine to process what state comes next. Selected unit may be null
        if (battleState == BattleState.DEFAULT || battleState == BattleState.START_STATE || battleState == BattleState.VIEWING_ENEMY)
        {
            //RaycastHit2D hit = Physics2D.Raycast(playerCursor.transform.position, new Vector2(1, 0), .1f, LayerMask.GetMask("CharacterLayer"));

            if (playerCursor.currentTile.isOccupied && ((playerCursor.currentTile.occupant.characterData.characterType == CHARACTER_TYPE.PLAYER && !playerHasMoved[playerCharacters.FindIndex(x => x == playerCursor.currentTile.occupant)]) || playerCursor.currentTile.occupant.characterData.characterType != CHARACTER_TYPE.PLAYER))
            {
                selectedUnit = playerCursor.currentTile.GetOccupant();
                //unitInfoController.SetPanel(selectedUnit);
            }

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
                    playerCursor.ToggleControls();
                    tileToMoveTo = moveableNode.tile;
                    
                    //selectedUnit.MoveCharacter(tileToMoveTo);
                    animationController.MoveCharacterToPosition(selectedUnit, tileToMoveTo);
                    UnityEngine.Object.Destroy(GameObject.Find("CurrentSelectableTiles"));

                    selectedUnit.SetNewTile(tileToMoveTo);
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
                case COLOR_TYPE.SUPPORT:
                    renderer.sprite = Resources.Load<Sprite>("Sprites/TileMaps/GlobalTiles/SupportTile") as Sprite;
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

        unitInfoController.DisablePanel();

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
        actionMenu.DisableUI();
        playerCursor.SwitchControlType("FieldBattle");
        playerCursor.MoveCursor(playerCursor.currentTile.transform.position, 1.0f);
        if (selectedUnit != null)
        {
            selectedUnit.SetNewTile(selectedUnit.previousTile);
            selectedUnit.MoveCharacter(selectedUnit.currentTile);
        }

        BattleStateMachine(stateQueue.Pop(), false);
    }

    #region ATTACK_ACTION

    public void StartAttackAction()
    {
        
    }

    public void AttackAction()
    {
        actionMenu.CreateItemPanel(selectedUnit.weaponList, selectedUnit.itemList, ACTION_BUTTON_LIST.ATTACK, selectedUnit);        
    }

    public void SelectTarget(WeaponData selectedWeapon)
    {
        actionMenu.CreateTargetList(selectedUnit.localTargets, selectedWeapon, selectedUnit);
    }

    public void ConfirmAttack(CharacterInfo target, WeaponData weaponData, bool needConfirmButton)
    {
        WeaponData targetWeapon = null;
        int targetDamage = 0;
        if(CanCounterAttack(selectedUnit, target))
        {
            targetWeapon = target.weaponList[0];
            targetDamage = CalculateDamage(target, selectedUnit, targetWeapon);
        }
        battlePreviewController.SetPanel(selectedUnit, target, weaponData, targetWeapon, CalculateDamage(selectedUnit, target, weaponData), targetDamage);
        if (needConfirmButton)
        {
            actionMenu.ConfirmAttack(target, weaponData);
        }
    }

    public void ResetPreviewPanel()
    {
        battlePreviewController.DisablePanel();
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

        playerCursor.ToggleControls();
        animationController.MoveCharacterToPosition(selectedUnit, tileToMoveTo);
        UnityEngine.Object.Destroy(GameObject.Find("CurrentSelectableTiles"));

        StartCoroutine(WaitForMove(weapon, target));
    }

    public IEnumerator WaitForMove(WeaponData weapon, CharacterInfo target)
    {
        yield return new WaitUntil(() => !animationController.isAnimating);

        playerCursor.ToggleControls();
        ConfirmAttack(target, weapon, true);
    }

    private int CalculateDamage(CharacterInfo attacker, CharacterInfo defender, WeaponData attackerWeapon)
    {
        int damage;
        if(attackerWeapon == null)
        {
            return 0;
        }
        if (attackerWeapon.isRanged)
        {
            switch (attackerWeapon.damageType)
            {
                case DAMAGE_TYPE.ENERGY:
                    damage = (attacker.characterData.PREC + attackerWeapon.ATK - (defender.characterData.SHIELD + defender.currentTile.defBoost));
                    break;
                case DAMAGE_TYPE.PHYSICAL:
                    damage = (attacker.characterData.PREC + attackerWeapon.ATK - (defender.characterData.ARMOR + defender.currentTile.defBoost));
                    break;
                default:
                    damage = (attacker.characterData.PREC + attackerWeapon.ATK - (defender.characterData.ARMOR + defender.currentTile.defBoost));
                    break;
            }
        }
        else
        {
            switch (attackerWeapon.damageType)
            {
                case DAMAGE_TYPE.ENERGY:
                    damage = (attacker.characterData.PHY + attackerWeapon.ATK - (defender.characterData.SHIELD + defender.currentTile.defBoost));
                    break;
                case DAMAGE_TYPE.PHYSICAL:
                    damage = (attacker.characterData.PHY + attackerWeapon.ATK - (defender.characterData.ARMOR + defender.currentTile.defBoost));
                    break;
                default:
                    damage = (attacker.characterData.PHY + attackerWeapon.ATK - (defender.characterData.ARMOR + defender.currentTile.defBoost));
                    break;
            }
        }
        return damage;
    }

    public void ProcessAttack(CharacterInfo selectedTarget, WeaponData selectedWeapon)
    {
        actionMenu.ResetMenus();
        unitInfoController.DisablePanel();
        battlePreviewController.DisablePanel();
        healthPanelController.SetPanel(selectedUnit, selectedTarget);
        targetedUnit = selectedTarget;

        int crit = 1;
        int playerHit,  playerCrit;
        int enemyDodge;

        bool attackHit;
        int damageDealt;

        playerHit = selectedUnit.characterData.HIT + selectedWeapon.HIT;
        playerCrit = selectedUnit.characterData.CRIT + selectedWeapon.CRIT;

        enemyDodge = selectedTarget.characterData.DODGE + selectedTarget.currentTile.dodgeBoost;

        if (attackHit = DoesAttackHit(playerHit, enemyDodge))
        {
            if (WorldStateInfo.Instance.GetNextRandomNumber() < playerCrit)
                crit = 3;
            if (selectedWeapon.isRanged)
            {
                damageDealt = ProcessDamageRanged(selectedUnit, selectedTarget, selectedWeapon, crit);
            }
            else
            {
                damageDealt = ProcessDamageMelee(selectedUnit, selectedTarget, selectedWeapon, crit);
            }     

        }
        else
        {
            damageDealt = -1;
        }

        PostDamageProcessing(selectedTarget, damageDealt);
        animationController.AddToAnimationQueue(selectedUnit, selectedTarget);

        if (CanCounterAttack(selectedUnit, selectedTarget) && !selectedTarget.flagIsDead)
        {
            damageDealt = ProcessEnemyAttack(selectedTarget, selectedUnit);
            PostDamageProcessing(selectedUnit, damageDealt);
            animationController.AddToAnimationQueue(selectedTarget, selectedUnit);
        }

        CharacterInfo secondAttack = ProcessSecondAttack(selectedUnit, selectedTarget);

        if(secondAttack != null && !secondAttack.flagIsDead && (!selectedUnit.flagIsDead && !selectedTarget.flagIsDead))
        {
            if (secondAttack == selectedUnit)
            {
                if (attackHit = DoesAttackHit(playerHit, enemyDodge))
                {
                    if (WorldStateInfo.Instance.GetNextRandomNumber() < playerCrit)
                        crit = 3;
                    if (selectedWeapon.isRanged)
                    {
                        damageDealt = ProcessDamageRanged(selectedUnit, selectedTarget, selectedWeapon, crit);
                    }
                    else
                    {
                        damageDealt = ProcessDamageMelee(selectedUnit, selectedTarget, selectedWeapon, crit);
                    }
                }
                PostDamageProcessing(selectedTarget, damageDealt);
                animationController.AddToAnimationQueue(selectedUnit, selectedTarget);
            }
            else if(secondAttack == selectedTarget)
            {
                damageDealt = ProcessEnemyAttack(selectedTarget, selectedUnit);
                PostDamageProcessing(selectedUnit, damageDealt);
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

        if (hitRoll < attackerHit - (defenderDodge))
        {
            return true;
        }
        else
            return false;
    }

    private int ProcessDamageRanged(CharacterInfo attackingUnit, CharacterInfo selectedTarget, WeaponData selectedWeapon, int crit)
    {
        int damageDealt;
        switch (selectedWeapon.damageType)
        {
            case DAMAGE_TYPE.ENERGY:
                damageDealt = ((attackingUnit.characterData.PREC + selectedWeapon.ATK * (crit)) - (selectedTarget.characterData.SHIELD + selectedTarget.currentTile.defBoost));
                selectedTarget.characterData.HP_CURRENT = selectedTarget.characterData.HP_CURRENT - damageDealt;
                Debug.Log((attackingUnit.characterData.PREC + selectedWeapon.ATK) + " Attack Power " + "Crit " + crit + " Defense " + (selectedTarget.characterData.SHIELD) + " Total Damage: " + damageDealt);
                break;
            case DAMAGE_TYPE.PHYSICAL:
                damageDealt = ((attackingUnit.characterData.PREC + selectedWeapon.ATK * (crit)) - (selectedTarget.characterData.ARMOR + selectedTarget.currentTile.defBoost));
                selectedTarget.characterData.HP_CURRENT = selectedTarget.characterData.HP_CURRENT - damageDealt;
                Debug.Log((attackingUnit.characterData.PREC + selectedWeapon.ATK) + " Attack Power " + "Crit " + crit + " Defense " + (selectedTarget.characterData.ARMOR) + " Total Damage: " + damageDealt);
                break;
            default:
                damageDealt = ((attackingUnit.characterData.PREC + selectedWeapon.ATK * (crit)) - (selectedTarget.characterData.ARMOR + selectedTarget.currentTile.defBoost));
                selectedTarget.characterData.HP_CURRENT = selectedTarget.characterData.HP_CURRENT - damageDealt;
                Debug.Log((attackingUnit.characterData.PREC + selectedWeapon.ATK) + " Attack Power " + "Crit " + crit + " Defense " + (selectedTarget.characterData.ARMOR) + " Total Damage: " + damageDealt);
                break;
        }
        return damageDealt;
    }

    private int ProcessDamageMelee(CharacterInfo attackingUnit, CharacterInfo selectedTarget, WeaponData selectedWeapon, int crit)
    {
        int damageDealt;
        switch (selectedWeapon.damageType)
        {
            case DAMAGE_TYPE.ENERGY:
                damageDealt = ((attackingUnit.characterData.PHY + selectedWeapon.ATK * (crit)) - (selectedTarget.characterData.SHIELD + selectedTarget.currentTile.defBoost));
                selectedTarget.characterData.HP_CURRENT = selectedTarget.characterData.HP_CURRENT - damageDealt;
                Debug.Log((attackingUnit.characterData.PHY + selectedWeapon.ATK) + " Attack Power " + "Crit " + crit + " Defense " + (selectedTarget.characterData.ARMOR) + " Total Damage: " + damageDealt);
                break;
            case DAMAGE_TYPE.PHYSICAL:
                damageDealt = ((attackingUnit.characterData.PHY + selectedWeapon.ATK * (crit)) - (selectedTarget.characterData.ARMOR + selectedTarget.currentTile.defBoost));
                selectedTarget.characterData.HP_CURRENT = selectedTarget.characterData.HP_CURRENT - damageDealt;
                Debug.Log((attackingUnit.characterData.PHY + selectedWeapon.ATK) + " Attack Power " + "Crit " + crit + " Defense " + (selectedTarget.characterData.ARMOR) + " Total Damage: " + damageDealt);
                break;
            default:
                damageDealt = ((attackingUnit.characterData.PHY + selectedWeapon.ATK * (crit)) - (selectedTarget.characterData.SHIELD + selectedTarget.currentTile.defBoost));
                selectedTarget.characterData.HP_CURRENT = selectedTarget.characterData.HP_CURRENT - damageDealt;
                Debug.Log((attackingUnit.characterData.PHY + selectedWeapon.ATK) + " Attack Power " + "Crit " + crit + " Defense " + (selectedTarget.characterData.ARMOR) + " Total Damage: " + damageDealt);
                break;
        }
        return damageDealt;
    }

    private bool CanCounterAttack(CharacterInfo attackingUnit, CharacterInfo defendingUnit)
    {
        defendingUnit.GetLocalTargetsCurrentWeapon();
        if(defendingUnit.localTargets.Exists(x=> x == attackingUnit) && !defendingUnit.flagIsDead)
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

    private void PostDamageProcessing(CharacterInfo defender, int damageDealt)
    {
        healthPanelController.AddToUpdateQueue(damageDealt, defender);
        if(defender.characterData.HP_CURRENT <= 0)
        {
            defender.characterData.HP_CURRENT = 0;
            defender.flagIsDead = true;
        }
    }

    public void ProcessHeal(CharacterInfo selectedTarget, WeaponData selectedWeapon)
    {
        healthPanelController.SetPanel(selectedUnit, selectedTarget);
        targetedUnit = selectedTarget;
        int damageHealed = 0;

        selectedTarget.characterData.HP_CURRENT = selectedTarget.characterData.HP_CURRENT + selectedWeapon.ATK;
        damageHealed = selectedWeapon.ATK;
        if(selectedTarget.characterData.HP_CURRENT > selectedTarget.characterData.HP)
        {
            damageHealed = selectedWeapon.ATK - (selectedUnit.characterData.HP_CURRENT - selectedTarget.characterData.HP);
            selectedTarget.characterData.HP_CURRENT = selectedTarget.characterData.HP;
        }

        PostDamageProcessing(selectedTarget, damageHealed);
        PostBattleCleanup();
    }

    public void UpdateHealthPanel()
    {
        healthPanelController.UpdatePanel();
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
            targetedUnit.currentTile.SetUnoccupied();
            targetedUnit.currentTile = null;
            targetedUnit.gameObject.SetActive(false);


        }
        if (selectedUnit.flagIsDead)
        {
            selectedUnit.currentTile.SetUnoccupied();
            selectedUnit.currentTile = null;
            selectedUnit.gameObject.SetActive(false);
        }
        healthPanelController.DisablePanel();
        IsThereAWinner();
        PostActionCleanup();
    }

    #endregion

    public void InventoryAction()
    {
        actionMenu.CreateItemPanel(selectedUnit.weaponList, selectedUnit.itemList, ACTION_BUTTON_LIST.INVENTORY, selectedUnit);
    }

    public void EquipWeapon(WeaponData weapon)
    {
        selectedUnit.ChangeWeapon(weapon);
        actionMenu.ReverseMenu();
        actionMenu.RebuildItemMenu(selectedUnit.weaponList, selectedUnit.itemList, ACTION_BUTTON_LIST.INVENTORY, selectedUnit);       
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

        unitInfoController.DisablePanel();

        playerHasMoved[playerCharacters.IndexOf(selectedUnit)] = true;
        selectedUnit = null;

        foreach(var character in playerCharacters)
        {
            //character.RecolorGraph();
            if(!character.flagIsDead)
                character.SetNewTile(character.currentTile);
        }
        foreach(var enemy in enemies)
        {
            if(!enemy.flagIsDead)
                enemy.SetNewTile(enemy.currentTile);
        }

        if (ToEnemyPhase())
        {
            //currentPhase = PHASE_LIST.ENEMY_PHASE;
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

    public void UnitsAction()
    {
        actionMenu.ResetMenus();
        actionMenu.DisableUI();
        unitWindow.ShowWindow();
    }

    public void DisableUnitWindow()
    {
        unitWindow.DisableWindow();
        BattleStateMachine(stateQueue.Pop(), false);
        playerCursor.SwitchControlType("FieldBattle");
    }

    public void EndTurnAction()
    {
        actionMenu.ResetMenus();
        playerCursor.SwitchControlType("FieldBattle");

        for (int i = 0; i < playerHasMoved.Length; i++)
        {
            playerHasMoved[i] = true;
        }

        BattleStateMachine(BattleState.CHANGE_PHASE);
    }


    public void SetWeaponInfoPanel(WeaponData weapon, GameObject weaponButtonObject)
    {
        weaponInfoController.SetPanel(weapon, weaponButtonObject);
        //weaponInfoController.GetComponent<RectTransform>().position = new Vector3(weaponButtonObject.transform.localPosition.x - 20, weaponButtonObject.transform.localPosition.y, weaponButtonObject.transform.localPosition.z);
    }

    public void DisableWeaponInfoPanel()
    {
        weaponInfoController.DisablePanel();
    }

    public void OpenUnitDetails()
    {
        if (playerCursor.currentTile.occupant != null && battleState != BattleState.UNIT_DETAILS)
        {
            playerCursor.SwitchControlType("UI");
            BattleStateMachine(BattleState.UNIT_DETAILS);
            unitDetailsWindow.BuildPanel(playerCursor.currentTile.occupant);
        }
        else if(battleState == BattleState.UNIT_DETAILS)
        {
            unitDetailsWindow.DisablePanel();
            playerCursor.SwitchControlType("FieldBattle");
            BattleStateMachine(stateQueue.Pop(), false);
        }
    }

    public void IsThereAWinner()
    {
        int playersDead = 0;
        int enemiesDead = 0;

        foreach(var player in playerCharacters)
        {
            if (player.flagIsDead)
                playersDead++;
        }
        
        if(playersDead == playerCharacters.Count)
        {
            ShowResultsScreen(RESULT_TYPE.DEFEAT);
            return;
        }

        foreach(var enemy in enemies)
        {
            if (enemy.flagIsDead)
                enemiesDead++;
        }

        if(enemiesDead == enemies.Count)
        {
            ShowResultsScreen(RESULT_TYPE.VICTORY);
            return;
        }
    }

    public void ShowResultsScreen(RESULT_TYPE result)
    {
        playerCursor.ToggleControls();

        resultsScreen.DisplayResult(result);

        healthPanelController.DisablePanel();
        tileInfoController.DisablePanel();
    }

    #region ENEMY_PHASE
    private void StartEnemyPhase()
    {
        playerCursor.ToggleControls();
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
                enemy.SetNewTile(enemy.currentTile);
                enemy.enemyAI.GetTarget();

                switch (enemy.enemyAI.chosenAction) {
                    case ENEMY_ACTIONS.ATTACK:                    
                        FindClosestTileAndAttack(enemy);
                        break;
                    case ENEMY_ACTIONS.MOVE:
                        MoveTowardTarget(enemy);
                        break;
                    case ENEMY_ACTIONS.WAIT:
                        EnemyCleanup(enemy);
                        break;
                    
                }
            }
            enemyHasMoved[enemies.IndexOf(enemy)] = true;
            yield return new WaitUntil(() => processNextEnemy);
        }

        StartPlayerTurn();
    }

    private void MoveTowardTarget(CharacterInfo enemy)
    {
        animationController.MoveCharacterToPosition(enemy, enemy.subGraph.graphNodes.Find(x => x.tile == enemy.enemyAI.chosenTile).tile);

        StartCoroutine(HoldEnemyAction(EnemyCleanup, enemy));

        enemy.SetNewTile(enemy.enemyAI.chosenTile);
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

        bool attackHit;

        playerHit = target.characterData.HIT + target.weaponList[0].HIT;
        playerDodge = target.characterData.DODGE + target.currentTile.dodgeBoost;
        playerCrit = target.characterData.CRIT + target.weaponList[0].CRIT;

        enemyHit = enemy.characterData.HIT + weapon.HIT;
        enemyDodge = enemy.characterData.DODGE + enemy.currentTile.dodgeBoost;
        enemyCrit = enemy.characterData.CRIT + weapon.CRIT;

        int crit = 1;

        int damageDealt;

        healthPanelController.SetPanel(enemy, target);

        if (attackHit = DoesAttackHit(enemyHit, playerDodge))
        {
            damageDealt = ProcessEnemyAttack(enemy, target);       
        }
        else
        {
            damageDealt = -1;
        }
        PostDamageProcessing(target, damageDealt);
        animationController.AddToAnimationQueue(enemy, target);

        if(CanCounterAttack(enemy, target))
        {
            if (attackHit = DoesAttackHit(playerHit, enemyDodge))
            {
                if (WorldStateInfo.Instance.GetNextRandomNumber() < playerCrit)
                    crit = 3;
                if (target.weaponList[0].isRanged)
                {
                    damageDealt = ProcessDamageRanged(target, enemy, target.weaponList[0], crit);
                }
                else
                {
                    damageDealt = ProcessDamageMelee(target, enemy, target.weaponList[0], crit);
                }      

            }
            else
            {
                damageDealt = -1;
            }
            PostDamageProcessing(enemy, damageDealt);
            animationController.AddToAnimationQueue(target, enemy);
        }

        CharacterInfo secondAttack = ProcessSecondAttack(enemy, target);

        if (secondAttack != null && !secondAttack.flagIsDead && (!enemy.flagIsDead && !target.flagIsDead))
        {
            if (secondAttack == target)
            {
                if (attackHit = DoesAttackHit(playerHit, enemyDodge))
                {
                    if (WorldStateInfo.Instance.GetNextRandomNumber() < playerCrit)
                        crit = 3;
                    if (target.weaponList[0].isRanged)
                    {
                        damageDealt = ProcessDamageRanged(target, enemy, target.weaponList[0], crit);
                    }
                    else
                    {
                        damageDealt = ProcessDamageMelee(target, enemy, target.weaponList[0], crit);
                    }
                }
                else
                {
                    damageDealt = -1;
                }
                PostDamageProcessing(enemy, damageDealt);
                animationController.AddToAnimationQueue(target, enemy);
                healthPanelController.AddToUpdateQueue(enemy.GetHPCurrent(), enemy);
            }
            else if (secondAttack == enemy)
            {
                damageDealt = ProcessEnemyAttack(enemy, target);
                  
                PostDamageProcessing(target, damageDealt);
                animationController.AddToAnimationQueue(enemy, target);
            }
            
        }

        animationController.AttackAnimationProcessing();

        StartCoroutine(HoldEnemyAction(EnemyCleanup, enemy));
        
    }

    private int ProcessEnemyAttack(CharacterInfo enemy, CharacterInfo target)
    {
        WeaponData weapon = enemy.weaponList[enemy.enemyAI.chosenWeapon];

        int playerDodge;
        int enemyHit, enemyCrit;

        bool attackHit;

        playerDodge = target.characterData.DODGE + target.currentTile.dodgeBoost;

        enemyHit = enemy.characterData.HIT + weapon.HIT;
        enemyCrit = enemy.characterData.CRIT + weapon.CRIT;

        int crit = 1;
        int damageDealt;

        if (attackHit = DoesAttackHit(enemyHit, playerDodge))
        {
            if (WorldStateInfo.Instance.GetNextRandomNumber() < enemyCrit)
                crit = 3;
            if (weapon.isRanged)
            {
                switch (weapon.damageType)
                {
                    case DAMAGE_TYPE.ENERGY:
                        damageDealt = ((enemy.characterData.PREC + weapon.ATK * (crit)) - target.characterData.SHIELD);
                        target.characterData.HP_CURRENT = target.characterData.HP_CURRENT - damageDealt;
                        Debug.Log((enemy.characterData.PREC + weapon.ATK) + " Attack Power " + "Crit " + crit + " Defense " + (target.characterData.SHIELD) + " Total Damage: " + damageDealt);
                        break;
                    case DAMAGE_TYPE.PHYSICAL:
                        damageDealt = ((enemy.characterData.PREC + weapon.ATK * (crit)) - target.characterData.ARMOR);
                        target.characterData.HP_CURRENT = target.characterData.HP_CURRENT - damageDealt;
                        Debug.Log((enemy.characterData.PREC + weapon.ATK) + " Attack Power " + "Crit " + crit + " Defense " + (target.characterData.ARMOR) + " Total Damage: " + damageDealt);
                        break;
                    default:
                        damageDealt = ((enemy.characterData.PREC + weapon.ATK * (crit)) - target.characterData.ARMOR);
                        target.characterData.HP_CURRENT = target.characterData.HP_CURRENT - damageDealt;
                        Debug.Log((enemy.characterData.PREC + weapon.ATK) + " Attack Power " + "Crit " + crit + " Defense " + (target.characterData.ARMOR) + " Total Damage: " + damageDealt);
                        break;
                }
            }
            else
            {
                switch (weapon.damageType)
                {
                    case DAMAGE_TYPE.ENERGY:
                        damageDealt = ((enemy.characterData.PHY + weapon.ATK * (crit)) - target.characterData.SHIELD);
                        target.characterData.HP_CURRENT = target.characterData.HP_CURRENT - damageDealt;
                        Debug.Log((enemy.characterData.PHY + weapon.ATK) + " Attack Power " + "Crit " + crit + " Defense " + (target.characterData.SHIELD) + " Total Damage: " + damageDealt);
                        break;
                    case DAMAGE_TYPE.PHYSICAL:
                        damageDealt = ((enemy.characterData.PHY + weapon.ATK * (crit)) - target.characterData.ARMOR);
                        target.characterData.HP_CURRENT = target.characterData.HP_CURRENT - damageDealt;
                        Debug.Log((enemy.characterData.PHY + weapon.ATK) + " Attack Power " + "Crit " + crit + " Defense " + (target.characterData.ARMOR) + " Total Damage: " + damageDealt);
                        break;
                    default:
                        damageDealt = ((enemy.characterData.PHY + weapon.ATK * (crit)) - target.characterData.ARMOR);
                        target.characterData.HP_CURRENT = target.characterData.HP_CURRENT - damageDealt;
                        Debug.Log((enemy.characterData.PHY + weapon.ATK) + " Attack Power " + "Crit " + crit + " Defense " + (target.characterData.ARMOR) + " Total Damage: " + damageDealt);
                        break;
                }
            }
        }
        else
        {
            damageDealt = -1;
        }
        //PostDamageProcessing(target, attackHit);
        return damageDealt;
    }

    private void EnemyCleanup(CharacterInfo enemy)
    {
        if (enemy.flagIsDead == true)
        {
            enemy.currentTile.SetUnoccupied();
            enemy.gameObject.SetActive(false);
        }
        if (enemy.enemyAI.chosenTarget.flagIsDead == true)
        {
            enemy.enemyAI.chosenTarget.currentTile.SetUnoccupied();
            enemy.enemyAI.chosenTarget.gameObject.SetActive(false);
        }

        healthPanelController.DisablePanel();



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
