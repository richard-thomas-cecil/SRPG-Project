using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
    public string tileName;
    public int movementCost;
    public int defBoost;
    public int dodgeBoost;

    public bool isOccupied;
    public CharacterInfo occupant;

    public int index;

    //Component References
    [HideInInspector]
    public BoxCollider2D bc2D;
    [HideInInspector]
    new public SpriteRenderer renderer;
    [HideInInspector]
    new public Transform transform;

    public GameObject eastTile;
    public GameObject westTile;
    public GameObject northTile;
    public GameObject southTile;


    void Awake()
    {
        bc2D = GetComponent<BoxCollider2D>();
        renderer = GetComponent<SpriteRenderer>();
        transform = GetComponent<Transform>();
        isOccupied = false;

        SetSurroundingTiles();
    }

    private void SetSurroundingTiles()
    {
        RaycastHit2D[] cast = new RaycastHit2D[1];
        
        if(bc2D.Raycast(new Vector2(1, 0), cast, 1f, LayerMask.GetMask("Tile")) != 0)
            eastTile = cast[0].collider.gameObject;

        
        if(bc2D.Raycast(new Vector2(-1, 0), cast, 1f, LayerMask.GetMask("Tile")) != 0)
            westTile = cast[0].collider.gameObject;

        if(bc2D.Raycast(new Vector2(0, 1), cast, 1f, LayerMask.GetMask("Tile")) != 0)
            northTile = cast[0].collider.gameObject;

        if(bc2D.Raycast(new Vector2(0, -1), cast, 1f, LayerMask.GetMask("Tile")) != 0)
            southTile = cast[0].collider.gameObject;
    }

    //When called, returns the appropriate tile
    public GameObject GetAdjacentTile(Vector2 direction)
    {
        if (direction == Vector2.right)
            return eastTile;
        if (direction == Vector2.left)
            return westTile;
        if (direction == Vector2.up)
            return northTile;
        if (direction == Vector2.down)
            return southTile;

        return null;
    }

    public void SetOccupied(CharacterInfo _occupant)
    {
        isOccupied = true;
        occupant = _occupant;
    }

    public void SetUnoccupied()
    {
        isOccupied = false;
        occupant = null;
    }

    public CharacterInfo GetOccupant()
    {
        if (occupant != null)
        {
            return occupant;
        }
        else
            return null;
    }
}
