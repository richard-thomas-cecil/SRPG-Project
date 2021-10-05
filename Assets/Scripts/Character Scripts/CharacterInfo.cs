using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Target_Struct
{
    public Target_Struct(GameObject _target, CHARACTER_TYPE _type)
    {
        target = _target;
        type = _type;
    }

    public GameObject target { get; }
    public CHARACTER_TYPE type { get; }
}

public class CharacterInfo : MonoBehaviour
{
    [SerializeField]private CharacterData baseCharacterData;
    [HideInInspector] public CharacterData characterData;
    //public Graph moveableTiles;
    public List<Node> moveableTiles;
    public Graph subGraph;

    public TileInfo currentTile;
    public TileInfo previousTile;

    public WeaponData[] weaponList = new WeaponData[4];
    public ItemData[] itemList = new ItemData[4];

    public List<CharacterInfo> targetList;
    public List<CharacterInfo> localTargets;

    public List<CharacterInfo> weaponTargets;                   //Used by StandardAI to check available targets of each weapon against all available targets

    public Dijsktras shortestPath;

    public StandardAI enemyAI;

    public bool flagIsDead;
    public bool canUseSupportWeapon;
    public bool canUseAttackWeapon;

    //Component References
    [HideInInspector]
    public BoxCollider2D bc2D;
    [HideInInspector]
    new public SpriteRenderer renderer;
    [HideInInspector]
    new public Transform transform;


    [SerializeField]private int minRange;
    [SerializeField]private int maxRange;

    private Rigidbody2D rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        targetList = new List<CharacterInfo>();
        localTargets = new List<CharacterInfo>();
        flagIsDead = false;
        canUseAttackWeapon = false;
        canUseSupportWeapon = false;
        rigidbody2D = GetComponent<Rigidbody2D>();

        bc2D = GetComponent<BoxCollider2D>();
        renderer = GetComponent<SpriteRenderer>();
        transform = GetComponent<Transform>();

        InitializeCharacterData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Node> GetGraph()
    {
        return moveableTiles;
    }

    private void InitializeCharacterData()
    {
        characterData = Instantiate(baseCharacterData);
        characterData.HP_CURRENT = characterData.HP;
        characterData.HIT = characterData.baseHit + (characterData.AIM * 2);
        characterData.DODGE = characterData.baseDodge + (characterData.SPEED * 2);
    }

    public void MoveCharacter(TileInfo newTile)
    {

        transform.position = newTile.transform.position;

        
    }

    public void SetNewTile(TileInfo newTile)
    {
        currentTile.SetUnoccupied();

        previousTile = currentTile;
        currentTile = newTile;

        moveableTiles.Clear();

        currentTile.SetOccupied(this);

        subGraph = WorldStateInfo.Instance.mapTileGraph.BuildSubGraph(currentTile.index, characterData.MOVE, minRange, maxRange, this);
        
        moveableTiles = subGraph.graphNodes;
        GetTargetList();
        

        RunDijsktras();
    }

    public void IntializeCharacter(GameObject startTile)
    {
        currentTile = startTile.GetComponent<TileInfo>();

        if (weaponList[0] != null)
        {
            minRange = weaponList[0].MINRANGE;
            maxRange = weaponList[0].MAXRANGE;
        }
        else
        {
            minRange = 0;
            maxRange = 0;
        }


        //subGraph = WorldStateInfo.Instance.mapTileGraph.BuildSubGraph(currentTile.index, characterData.MOVE, minRange, maxRange);

        //moveableTiles = subGraph.graphNodes;
        //moveableTiles.BuildGraph(currentTile, characterData.MOVE, minRange, maxRange);

        moveableTiles = new List<Node>();

        //Debug.Log("SubGraph for " + this.gameObject);
        //foreach(var node in subGraph)
        //{
        //    Debug.Log(node.tile);
        //}

        shortestPath = new Dijsktras(subGraph);

        transform.position = currentTile.transform.position;
        currentTile.GetComponent<TileInfo>().SetOccupied(this);
    }

    //Gets all valid targets before movement
    public void GetTargetList()
    {
        targetList.Clear();
        foreach(var tile in moveableTiles)
        {
            if(tile.tile.isOccupied)
            {
                targetList.Add(tile.tile.occupant);
            }
        }
    }

    //Get nearby Targets post movement
    public void GetLocalTargets()
    {
        Graph localTargetSubGraph = WorldStateInfo.Instance.mapTileGraph.BuildSubGraph(currentTile.GetComponent<TileInfo>().index, 0, minRange, maxRange, this);
        
        localTargets.Clear();
        canUseAttackWeapon = false;
        canUseSupportWeapon = false;

        foreach (var tile in localTargetSubGraph.graphNodes)
        {
            if (tile.tile.isOccupied)
            {
                if (EvaluateIsEnemy(tile.tile.occupant) && (tile.colorType == COLOR_TYPE.MOVEMENT || tile.colorType == COLOR_TYPE.ATTACK))
                {
                    localTargets.Add(tile.tile.occupant);
                    canUseAttackWeapon = true;
                }
                else if (!EvaluateIsEnemy(tile.tile.occupant) && tile.colorType == COLOR_TYPE.SUPPORT && tile.tile.occupant.characterData.HP_CURRENT < tile.tile.occupant.characterData.HP)
                {
                    localTargets.Add(tile.tile.occupant);
                    canUseSupportWeapon = true;
                }
            }
        }
        //foreach(var tile in localSupportTargetSubGraph.graphNodes)
        //{
        //    RaycastHit2D hit = Physics2D.Raycast(tile.tile.transform.position, new Vector2(0, 1), 0.1f, LayerMask.GetMask("CharacterLayer"));
        //    if (hit.collider != null && hit.collider.gameObject != this.gameObject && hit.collider.gameObject.GetComponent<CharacterInfo>().characterData.characterType == CHARACTER_TYPE.PLAYER)
        //    {
        //        switch (characterData.characterType)
        //        {
        //            case CHARACTER_TYPE.PLAYER:
        //                if (hit.collider.gameObject.GetComponent<CharacterInfo>().characterData.characterType != CHARACTER_TYPE.ENEMY)
        //                    localTargets.Add(hit.collider.gameObject.GetComponent<CharacterInfo>()); ;
        //                break;
        //            case CHARACTER_TYPE.ENEMY:
        //                if (hit.collider.gameObject.GetComponent<CharacterInfo>().characterData.characterType == CHARACTER_TYPE.ENEMY)
        //                    localTargets.Add(hit.collider.gameObject.GetComponent<CharacterInfo>());
        //                break;
        //            case CHARACTER_TYPE.OTHER:
        //                if (hit.collider.gameObject.GetComponent<CharacterInfo>().characterData.characterType != CHARACTER_TYPE.ENEMY)
        //                    localTargets.Add(hit.collider.gameObject.GetComponent<CharacterInfo>());
        //                break;
        //        }
        //    }
        //}
    }

    public void GetLocalTargets(TileInfo newTile)
    {
        Graph localTargetSubGraph = WorldStateInfo.Instance.mapTileGraph.BuildSubGraph(newTile.index, 0, minRange, maxRange, this);

        localTargets.Clear();
        canUseAttackWeapon = false;
        canUseSupportWeapon = false;

        foreach (var tile in localTargetSubGraph.graphNodes)
        {
            if (tile.tile.isOccupied)
            {
                if (EvaluateIsEnemy(tile.tile.occupant) && (tile.colorType == COLOR_TYPE.MOVEMENT || tile.colorType == COLOR_TYPE.ATTACK))
                {
                    localTargets.Add(tile.tile.occupant);
                    canUseAttackWeapon = true;
                }
                else if (!EvaluateIsEnemy(tile.tile.occupant) && tile.inSupportRange && tile.tile.occupant.characterData.HP_CURRENT < tile.tile.occupant.characterData.HP)
                {
                    localTargets.Add(tile.tile.occupant);
                    canUseSupportWeapon = true;
                }
            }
        }
    }

    public void GetLocalTargetsCurrentWeapon()
    {
        Graph localTargetSubGraph = WorldStateInfo.Instance.mapTileGraph.BuildSubGraph(currentTile.GetComponent<TileInfo>().index, 0, weaponList[0].MINRANGE, weaponList[0].MAXRANGE, this);

        localTargets.Clear();
        canUseAttackWeapon = false;
        canUseSupportWeapon = false;

        foreach (var tile in localTargetSubGraph.graphNodes)
        {
            if (tile.tile.isOccupied)
            {
                if (EvaluateIsEnemy(tile.tile.occupant) && (tile.colorType == COLOR_TYPE.MOVEMENT || tile.colorType == COLOR_TYPE.ATTACK))
                {
                    localTargets.Add(tile.tile.occupant);
                    canUseAttackWeapon = true;
                }
                else if (tile.tile.occupant != this && !EvaluateIsEnemy(tile.tile.occupant) && tile.colorType == COLOR_TYPE.SUPPORT && tile.tile.occupant.characterData.HP_CURRENT < tile.tile.occupant.characterData.HP)
                {
                    canUseSupportWeapon = true;
                    localTargets.Add(tile.tile.occupant);
                }
            }
        }
    }

    public void RecolorGraph()
    {
        subGraph.RecolorGraph(GetMOVE(), minRange, maxRange, this);
    }

    public TileInfo FindBestTile(WeaponData chosenWeapon, CharacterInfo chosenTarget)
    {
        Dijsktras dijsktras = new Dijsktras(WorldStateInfo.Instance.mapTileGraph);
        List<Node> tilesToEvaluate = new List<Node>();
        Graph tilesInRange = new Graph();
        int range = chosenWeapon.MAXRANGE;
        tilesInRange = WorldStateInfo.Instance.mapTileGraph.BuildSubGraph(chosenTarget.currentTile.index, 0, chosenWeapon.MINRANGE, range, chosenTarget);

        TileInfo chosenTile = null;

        foreach (var tile in tilesInRange.graphNodes)
        {
            Node nextTile = subGraph.graphNodes.Find(x => x.tile == tile.tile);
            
            if (nextTile != null && nextTile.colorType == COLOR_TYPE.MOVEMENT)
            {
                nextTile.weight = tile.weight;
                tilesToEvaluate.Add(nextTile);
            }
        }

        while (chosenTile == null && tilesToEvaluate.Count > 0 && range >= chosenWeapon.MINRANGE)
        {
            chosenTile = EvaluateTileAtRange(range, tilesToEvaluate);
            tilesToEvaluate.RemoveAll(x => x.weight == range);
            range = range - 1;
        }

        return chosenTile;
    }

    private TileInfo EvaluateTileAtRange(int range, List<Node> tilesToEvaluate)
    {
        TileInfo chosenTile = null;
        foreach (var tile in tilesToEvaluate)
        {
            if (tile.weight == range && (!tile.tile.isOccupied || tile.tile.occupant == this))
            {
                if (chosenTile == null || (shortestPath.dist[tile.tile.index] < shortestPath.dist[chosenTile.index]))
                    chosenTile = tile.tile;
            }
        }
        return chosenTile;
    }

    //Used to indicate if evaluationTarget is an enemy of this unit
    public bool EvaluateIsEnemy(CharacterInfo evaluationTarget)
    {
        if(evaluationTarget == null)
        {
            return false;
        }
        switch (characterData.characterType)
        {
            case CHARACTER_TYPE.PLAYER:
                if (evaluationTarget.characterData.characterType == CHARACTER_TYPE.PLAYER || evaluationTarget.characterData.characterType == CHARACTER_TYPE.OTHER)
                    return false;
                break;
            case CHARACTER_TYPE.ENEMY:
                if (evaluationTarget.characterData.characterType == CHARACTER_TYPE.ENEMY)
                    return false;
                break;
            default:
                break;
        }
        return true;
    }

    public void RunDijsktras()
    {
        //shortestPath.ReplaceGraph(subGraph);
        shortestPath.DijsktrasAlogrithm(currentTile.index, this);
    }

    #region STAT_GETTERS
    public int GetMaxHP()
    {
        return characterData.HP;
    }
    
    public int GetHPCurrent()
    {
        return characterData.HP_CURRENT;
    }

    public int GetPHY()
    {
        return characterData.PHY;
    }

    public int GetPREC()
    {
        return characterData.PREC;
    }

    public int GetAIM()
    {
        return characterData.AIM;
    }

    public int GetARMOR()
    {
        return characterData.ARMOR;
    }

    public int GetSHIELD()
    {
        return characterData.SHIELD;
    }

    public int GetSPEED()
    {
        return characterData.SPEED;
    }

    public int GetMOVE()
    {
        return characterData.MOVE;
    }

    public int GetMaxRange()
    {
        return maxRange;
    }

    public int GetMinRange()
    {
        return minRange;
    }
    #endregion

   public void ChangeWeapon(WeaponData weapon)
    {
        WeaponData tempWeapon = weaponList[0];
        int weaponIndex = 0;
        for(int i = 0; i < weaponList.Length; i++)
        {
            if (weaponList[i] == weapon)
                weaponIndex = i;
        }

        weaponList[0] = weapon;
        weaponList[weaponIndex] = tempWeapon;
    }

    public Rigidbody2D GetRigidbody2D()
    {
        return rigidbody2D;
    }
}
