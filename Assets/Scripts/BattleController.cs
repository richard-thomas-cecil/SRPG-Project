using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



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

    private PlayerController playerCursor;
    private ActionsMenuController actionMenu;

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        playerCharacters = new List<CharacterInfo>();
        playerCursor = GameObject.Find("Player").GetComponent<PlayerController>();

        actionMenu = WorldStateInfo.Instance.actionMenu.GetComponent<ActionsMenuController>();

        mapInfo = WorldStateInfo.Instance.currentMapInfo;
        currentPhase = PHASE_LIST.PLAYER_PHASE;
        battleState = BattleState.SELECTING_UNIT;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BattleStateMachine()
    {
        switch (battleState)
        {
            case BattleState.SELECTING_UNIT:
                if(selectedUnit == null || (selectedUnit.characterData.characterType == CHARACTER_TYPE.PLAYER && playerHasMoved[playerCharacters.IndexOf(selectedUnit)]))
                {
                    battleState = BattleState.SELECT_ACTION;                       
                }
                else if(selectedUnit.characterData.characterType == CHARACTER_TYPE.PLAYER)
                {
                    ColorTiles(selectedUnit.GetGraph());
                    battleState = BattleState.MOVE_UNIT;
                }
                else
                {
                    ColorTilesEnemy(selectedUnit.GetGraph());
                    battleState = BattleState.VIEWING_ENEMY;
                }
                break;
            case BattleState.MOVE_UNIT:
                battleState = BattleState.SELECT_ACTION;
                break;
            case BattleState.VIEWING_ENEMY:
                battleState = BattleState.SELECTING_UNIT;
                break;
            case BattleState.SELECT_ACTION:
                battleState = BattleState.SELECTING_UNIT;
                break;
            default:
                battleState = BattleState.SELECTING_UNIT;
                break;
        }
    }

    private void ChangeBattleState()
    {

    }

    public void SelectTile()
    {
        if (battleState == BattleState.SELECTING_UNIT || battleState == BattleState.VIEWING_ENEMY)
        {
            //RaycastHit2D hit = Physics2D.Raycast(playerCursor.transform.position, new Vector2(1, 0), .1f, LayerMask.GetMask("CharacterLayer"));

            selectedUnit = playerCursor.currentTile.GetOccupant();

            if (GameObject.Find("CurrentSelectableTiles") != null)
            {
                Object.Destroy(GameObject.Find("CurrentSelectableTiles"));
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
            BattleStateMachine();
            
            return;
        }
        if (battleState == BattleState.MOVE_UNIT)
        {
            RaycastHit2D hit = Physics2D.Raycast(playerCursor.transform.position, new Vector2(1, 0), .1f, LayerMask.GetMask("SelectableTile"));
            
            if (hit.collider != null)
            {
                hit = Physics2D.Raycast(playerCursor.transform.position, new Vector2(1, 0), .1f, LayerMask.GetMask("Tile"));
                Node moveableNode = selectedUnit.moveableTiles.Find(x => x.tile == hit.collider.gameObject);
                if (moveableNode.colorType == COLOR_TYPE.MOVEMENT)
                {
                    selectedUnit.MoveCharacter(hit.collider.gameObject);
                    Object.Destroy(GameObject.Find("CurrentSelectableTiles"));

                    selectedUnit.GetLocalTargets();

                    BuildActionMenu();
                }
                else if (moveableNode.colorType == COLOR_TYPE.ATTACK && hit.collider.GetComponentInParent<TileInfo>().isOccupied)
                {
                    int closestTile = FindClosestTile(hit.collider.gameObject);
                    selectedUnit.MoveCharacter(selectedUnit.moveableTiles.Find(x => x.nodeIndex == closestTile).tile);

                    Object.Destroy(GameObject.Find("CurrentSelectableTiles"));
                }
                BattleStateMachine();
            }
        }
    }

    public void ColorTiles(List<Node> moveableTiles)
    {
        List<GameObject> movementTiles = new List<GameObject>();

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
    }

    private void ColorTilesEnemy(List<Node> moveableTiles)
    {
        List<GameObject> movementTiles = new List<GameObject>();

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
    }

    private List<GameObject> AddRangeTiles(Node tile)
    {
        List<GameObject> rangeTiles = new List<GameObject>();
        foreach (GameObject rangeTile in tile.tilesInRange)
        {
            rangeTiles.Add(rangeTile);
        }

        return rangeTiles;
    }

    private int FindClosestTile(GameObject target)
    {
        List<Node> graph = selectedUnit.GetGraph();
        COLOR_TYPE currentTileColor;
        int targetIndex = graph.Find(x => x.tile == target).nodeIndex;
        int currentTile = targetIndex;

        do
        {
            currentTile = selectedUnit.shortestPath.parent[currentTile];
            currentTileColor = graph.Find(x => x.nodeIndex == currentTile).colorType;
        } while (currentTileColor != COLOR_TYPE.MOVEMENT);

        return currentTile;
    }

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
    }

    private void DefaultActionMenu()
    {

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
    }

    public void WaitAction()
    {
        int selectedUnitIndex = playerCharacters.FindIndex(x=> x == selectedUnit);
        playerHasMoved[selectedUnitIndex] = true;

        BattleStateMachine();
    }
}
