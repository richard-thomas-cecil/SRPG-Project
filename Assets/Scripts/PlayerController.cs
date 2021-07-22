using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    private PlayerControls controls;

    private int frameWait;
    private int frameMoveWait;
    private int moveHold;

    private CharacterInfo selectedUnit;

    public TileInfo currentTile;

    public CharacterData character;

    private BasicMapInfo thisMap;

    void Awake()
    {
        rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        frameWait = 0;
        frameMoveWait = 0;
        moveHold = 0;
        controls = new PlayerControls();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        thisMap = GameObject.Find("/MapInfo").GetComponent<BasicMapInfo>();
        currentTile = thisMap.startingPositions[0].GetComponent<TileInfo>();
        transform.position = currentTile.transform.position;


        controls.FieldBattle.Select.canceled += _ => WorldStateInfo.Instance.battleController.SelectTile();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Enable();
    }

    // Update is called once per frame
    void Update()
    {
    }

    

    void FixedUpdate()
    {
        if (WorldStateInfo.Instance.battleController.battleState == BattleState.SELECT_ACTION) return;
        float horizontal = controls.FieldBattle.Horizontal.ReadValue<float>();
        float vertical = controls.FieldBattle.Vertical.ReadValue<float>();
        if(horizontal != 0 || vertical != 0)
        {
            MovePlayer();
            frameMoveWait = frameMoveWait + 1;
        }
        else
        {
            moveHold = 0;
            frameMoveWait = 0;
        }
        frameWait = frameWait + 1;
        if(frameWait > 60)
        {
            frameWait = 0;
        }
    }

    #region MOVEMENT
    public void MovePlayer()
    {
        int movementWaitTime;

        if(moveHold >= 3)
        {
            movementWaitTime = 5;
        }
        else
        {
            movementWaitTime = 20;
        }
        if (frameMoveWait == 0 || frameMoveWait % movementWaitTime == 0)
        {
            //float horizontalMove = Input.GetAxisRaw("Horizontal");
            //float verticalMove = Input.GetAxisRaw("Vertical");

            Vector3 position = transform.position;

            float horizontal = controls.FieldBattle.Horizontal.ReadValue<float>();
            float vertical = controls.FieldBattle.Vertical.ReadValue<float>();

            TileInfo newTile = null;

            //position.x = position.x + (GetComponent<SpriteRenderer>().bounds.size.x * horizontalMove);
            //position.y = position.y + GetComponent<SpriteRenderer>().bounds.size.y * verticalMove;

            if(horizontal != 0)
            {
                if (currentTile.GetAdjacentTile(new Vector2(horizontal, 0)) != null)
                    newTile = currentTile.GetAdjacentTile(new Vector2(horizontal, 0)).GetComponent<TileInfo>();
            }
            else if(vertical != 0)
            {
                if(currentTile.GetAdjacentTile(new Vector2(0, vertical)) != null)
                    newTile = currentTile.GetAdjacentTile(new Vector2(0, vertical)).GetComponent<TileInfo>();
            }

            if(newTile != null)
            {
                position = newTile.transform.position;
                currentTile = newTile;
            }

            StartCoroutine(SmoothMovement(position));
                                    
            if(moveHold < 4)
            {
                moveHold = moveHold + 1;
            }
        }
    }

    private IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rigidBody2D.position, end, .25f);

            rigidBody2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            yield return null;
        }
    }
    #endregion

    //public void SelectTile()
    //{
    //    if (WorldStateInfo.Instance.GetBattleState() == BattleState.SELECTING_UNIT)
    //    {
    //        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(1, 0), .5f, LayerMask.GetMask("CharacterLayer"));
    //        if (GameObject.Find("CurrentSelectableTiles") != null)
    //        {
    //            Object.Destroy(GameObject.Find("CurrentSelectableTiles"));
    //        }
    //        if (hit.collider != null)
    //        {
    //            hit.collider.GetComponentInParent<CharacterInfo>().GetTargetList();
    //            ColorTiles(hit.collider.GetComponentInParent<CharacterInfo>().GetGraph());
    //            WorldStateInfo.Instance.battleMode = BattleMode.MOVE_UNIT;
    //            selectedUnit = hit.collider.gameObject.GetComponent<CharacterInfo>();
                
    //        }
    //        return;
    //    }
    //    if(WorldStateInfo.Instance.GetBattleState() == BattleMState.MOVE_UNIT)
    //    {
    //        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(1, 0), .5f, LayerMask.GetMask("SelectableTile"));
    //        if(hit.collider != null)
    //        {
    //            hit = Physics2D.Raycast(transform.position, new Vector2(1, 0), .5f, LayerMask.GetMask("Tile"));
    //            Node moveableNode = selectedUnit.moveableTiles.Find(x => x.tile == hit.collider.gameObject);
    //            if (moveableNode.colorType == COLOR_TYPE.MOVEMENT)
    //            {
    //                selectedUnit.MoveCharacter(hit.collider.gameObject);
    //                Object.Destroy(GameObject.Find("CurrentSelectableTiles"));

    //                WorldStateInfo.Instance.battleMode = BattleMode.SELECTING_UNIT;
    //            }
    //            if (moveableNode.colorType == COLOR_TYPE.ATTACK && hit.collider.GetComponentInParent<TileInfo>().isOccupied)
    //            {
    //                int closestTile = FindClosestTile(hit.collider.gameObject);
    //                selectedUnit.MoveCharacter(selectedUnit.moveableTiles.Find(x => x.nodeIndex == closestTile).tile);
    //                Object.Destroy(GameObject.Find("CurrentSelectableTiles"));

    //                WorldStateInfo.Instance.battleMode = BattleMode.SELECTING_UNIT;
    //            }
    //        }
    //    }
    //}

    //public void ColorTiles(List<Node> moveableTiles)
    //{
    //    List<GameObject> movementTiles = new List<GameObject>();

    //    GameObject currentSelectableTiles = new GameObject("CurrentSelectableTiles");
    //    string objectName = "Selectable";
    //    int index = 0;
    //    foreach (Node tile in moveableTiles)
    //    {
    //        GameObject moveableTileSprite;
    //        moveableTileSprite = new GameObject(objectName + index);
    //        moveableTileSprite.layer = 9;

    //        movementTiles.Add(tile.tile);

    //        SpriteRenderer renderer = moveableTileSprite.AddComponent<SpriteRenderer>();
    //        switch (tile.colorType)
    //        {
    //            case COLOR_TYPE.MOVEMENT:
    //                renderer.sprite = Resources.Load<Sprite>("Sprites/TileMaps/GlobalTiles/MoveableTile") as Sprite;
    //                break;
    //            case COLOR_TYPE.ATTACK:
    //                renderer.sprite = Resources.Load<Sprite>("Sprites/TileMaps/GlobalTiles/RangeTile") as Sprite;
    //                break;
    //            default:
    //                break;

    //        }
    //        moveableTileSprite.transform.parent = currentSelectableTiles.transform;

    //        BoxCollider2D collider = moveableTileSprite.AddComponent<BoxCollider2D>();

    //        moveableTileSprite.transform.position = tile.tile.transform.position;
    //        index++;
    //    }
    //}

    //private List<GameObject> AddRangeTiles(Node tile)
    //{
    //    List<GameObject> rangeTiles = new List<GameObject>();
    //    foreach(GameObject rangeTile in tile.tilesInRange)
    //    {
    //        rangeTiles.Add(rangeTile);
    //    }

    //    return rangeTiles;
    //}

    //private int FindClosestTile(GameObject target)
    //{
    //    List<Node> graph = selectedUnit.GetGraph();
    //    COLOR_TYPE currentTileColor;
    //    int targetIndex = graph.Find(x => x.tile == target).nodeIndex;
    //    int currentTile = targetIndex;

    //    do
    //    {
    //        currentTile = selectedUnit.shortestPath.parent[currentTile];
    //        currentTileColor = graph.Find(x => x.nodeIndex == currentTile).colorType;
    //    } while (currentTileColor != COLOR_TYPE.MOVEMENT);

    //    return currentTile;
    //}
}
