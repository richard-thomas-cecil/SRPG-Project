using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The "Color" of a tile corresponds to:
//MOVMENT: Can the unit move onto this space?
//ATTACK: Is this in attack range of the unit
//Support: Is this in the range of a units support skills?
//NONE: Not in any range of unit
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
    public COLOR_TYPE colorType;
    public bool visited;
    public int nodeIndex;
    public int weight;
    public bool inSupportRange;

    public Node(TileInfo thisTile, int thisIndex)
    {
        tile = thisTile.GetComponent<TileInfo>();
        nodeIndex = thisIndex;
        colorType = COLOR_TYPE.NONE;
        inSupportRange = false;
    }

    public void MarkVisited()
    {
        visited = true;
    }

}
