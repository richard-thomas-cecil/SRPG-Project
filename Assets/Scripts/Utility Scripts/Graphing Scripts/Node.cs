using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum COLOR_TYPE
{
    MOVEMENT,
    ATTACK,
    SUPPORT,
    NONE
}
public class Node
{
    public TileInfo tile;
    public List<GameObject> tilesInRange;     //Tiles in attack range of this tile
    public COLOR_TYPE colorType;
    public bool visited;
    public int nodeIndex;

    public Node(TileInfo thisTile, int thisIndex)
    {
        tile = thisTile.GetComponent<TileInfo>();
        nodeIndex = thisIndex;
        colorType = COLOR_TYPE.NONE;
    }

    public void MarkVisited()
    {
        visited = true;
    }

    public void AddRangeTile(GameObject tileToAdd)
    {
        tilesInRange.Add(tileToAdd);
    }
}
