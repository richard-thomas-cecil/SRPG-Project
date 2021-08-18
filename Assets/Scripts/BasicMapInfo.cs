using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMapInfo : MonoBehaviour
{
    public List<GameObject> startingPositions;
    public List<CharacterInfo> playerTeam;
    public List<CharacterInfo> enemies;
    public List<GameObject> enemyPositions;
    public Graph mapGraph;

    // Start is called before the first frame update
    void Start()
    {
        //mapGraph = new Graph();
        //mapGraph.BuildGraph(startingPoint);
        IntializeMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IntializeMap()
    {
        

        mapGraph = WorldStateInfo.Instance.mapTileGraph;

        foreach(var node in mapGraph.graphNodes)
        {
            node.tile.GetComponent<TileInfo>().index = node.nodeIndex;
            node.tile.name = "Tile " + node.nodeIndex;
        }

        foreach (var enemy in enemies)
        {
            enemy.IntializeCharacter(enemyPositions[enemies.IndexOf(enemy)]);
            //enemy.RunDijsktras();
        }

        foreach (var pc in playerTeam)
        {

            pc.IntializeCharacter(startingPositions[playerTeam.IndexOf(pc)]);
            //pc.RunDijsktras();
        }

        foreach(var enemy in enemies)
        {
            enemy.SetNewTile(enemy.currentTile);
        }

        foreach (var pc in playerTeam)
        {
            pc.SetNewTile(pc.currentTile);
        }

        //InitializeTargets();
    }

    //private void InitializeTargets()
    //{
    //    foreach (var enemy in enemies)
    //    {
    //        enemy.GetComponent<CharacterInfo>().GetTargetList();
    //    }
    //    playerTeam.GetComponent<CharacterInfo>().GetTargetList();
    //}
}
