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
    public CharacterData characterData;
    //public Graph moveableTiles;
    public List<Node> moveableTiles;
    public Graph subGraph;

    public TileInfo currentTile;

    public WeaponData[] weaponList = new WeaponData[4];

    public List<Target_Struct> targetList;
    public List<Target_Struct> localTargets;

    public Dijsktras shortestPath;

    //Component References
    [HideInInspector]
    public BoxCollider2D bc2D;
    [HideInInspector]
    new public SpriteRenderer renderer;
    [HideInInspector]
    new public Transform transform;


    private int minRange;
    private int maxRange;

    // Start is called before the first frame update
    void Start()
    {
        targetList = new List<Target_Struct>();
        localTargets = new List<Target_Struct>();

        bc2D = GetComponent<BoxCollider2D>();
        renderer = GetComponent<SpriteRenderer>();
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Node> GetGraph()
    {
        return moveableTiles;
    }

    public void MoveCharacter(TileInfo newTile)
    {

        transform.position = newTile.transform.position;

        currentTile.SetUnoccupied();

        currentTile = newTile;

        moveableTiles.Clear();

        subGraph = WorldStateInfo.Instance.mapTileGraph.BuildSubGraph(currentTile.index, characterData.MOVE, minRange, maxRange);

        moveableTiles = subGraph.graphNodes;
        GetTargetList();
        currentTile.SetOccupied(this);

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


        subGraph = WorldStateInfo.Instance.mapTileGraph.BuildSubGraph(currentTile.index, characterData.MOVE, minRange, maxRange);

        moveableTiles = subGraph.graphNodes;
        //moveableTiles.BuildGraph(currentTile, characterData.MOVE, minRange, maxRange);

        

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
        foreach(var tile in moveableTiles)
        {
            RaycastHit2D hit = Physics2D.Raycast(tile.tile.transform.position, new Vector2(0, 1), 0.1f, LayerMask.GetMask("CharacterLayer"));
            if(hit.collider != null && hit.collider.gameObject != this.gameObject)
            {
                Target_Struct newTarget = new Target_Struct(hit.collider.gameObject, CHARACTER_TYPE.ENEMY);
                targetList.Add(newTarget);
            }
        }
    }

    //Get nearby Targets post movement
    public void GetLocalTargets()
    {
        Graph localTargetSubGraph = WorldStateInfo.Instance.mapTileGraph.BuildSubGraph(currentTile.GetComponent<TileInfo>().index, 0, minRange, maxRange);
        Graph localSupportTargetSubGraph = WorldStateInfo.Instance.mapTileGraph.BuildSubGraph(currentTile.GetComponent<TileInfo>().index, 0, characterData.supportRangeMin, characterData.supportRangeMax);
        localTargets.Clear();

        foreach(var tile in localTargetSubGraph.graphNodes)
        {
            RaycastHit2D hit = Physics2D.Raycast(tile.tile.transform.position, new Vector2(0, 1), 0.1f, LayerMask.GetMask("CharacterLayer"));
            if (hit.collider != null && hit.collider.gameObject != this.gameObject && hit.collider.gameObject.GetComponent<CharacterInfo>().characterData.characterType == CHARACTER_TYPE.ENEMY)
            {
                Target_Struct newTarget = new Target_Struct(hit.collider.gameObject, CHARACTER_TYPE.ENEMY);
                localTargets.Add(newTarget);
            }
        }
        foreach(var tile in localSupportTargetSubGraph.graphNodes)
        {
            RaycastHit2D hit = Physics2D.Raycast(tile.tile.transform.position, new Vector2(0, 1), 0.1f, LayerMask.GetMask("CharacterLayer"));
            if (hit.collider != null && hit.collider.gameObject != this.gameObject && hit.collider.gameObject.GetComponent<CharacterInfo>().characterData.characterType == CHARACTER_TYPE.PLAYER)
            {
                Target_Struct newTarget = new Target_Struct(hit.collider.gameObject, CHARACTER_TYPE.PLAYER);
                localTargets.Add(newTarget);
            }
        }
    }

    public void RunDijsktras()
    {
        shortestPath.ReplaceGraph(subGraph);
        shortestPath.DijsktrasAlogrithm(currentTile.index);
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
    #endregion
}
