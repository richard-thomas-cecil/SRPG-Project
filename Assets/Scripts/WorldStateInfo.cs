using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Contains Global information to be displayed to all objects in scene
//Implements Singleton pattern
public class WorldStateInfo : MonoBehaviour
{
    private static WorldStateInfo _instance;

    public static WorldStateInfo Instance { get { return _instance; } }

    public PlayerMode playMode;                     //current mode of play. see enumerations.cs for more info

    public BasicMapInfo currentMapInfo;             //Map Data. All objects should pull map information they need from here

    public Graph mapTileGraph;                      //Main graph that all objects should reference for any graph related needs, including the building of subgraphs

    public BattleController battleController;       //Battle controller. BattleController script should be attached to same object as this script.

    public GameObject actionMenu;                   //Probably going to remove this

    public List<GameObject> UI_Prefabs;             //TBD

    void Awake()
    {
        //Creates new singleton instance if one is not already created
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
                
        currentMapInfo = GameObject.Find("MapInfo").GetComponent<BasicMapInfo>();

        playMode = PlayerMode.FIELD_BATTLE;

        //BuildGraph from current mapInfo
        mapTileGraph = new Graph();
        mapTileGraph.BuildGraph(currentMapInfo.startingPositions[0]);
        actionMenu = GameObject.Find("ActionMenu");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Pulls battleState from battle Controller.
    //Mostly exsists to cleanup the code a little
    public BattleState GetBattleState()
    {
        return battleController.battleState;
    }
}
