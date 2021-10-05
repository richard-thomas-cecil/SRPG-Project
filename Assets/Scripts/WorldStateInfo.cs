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
    public Dijsktras dijsktrasFullMap;

    public BattleController battleController;       //Battle controller. BattleController script should be attached to same object as this script.

    public GameObject actionMenu;                   
    public GameObject unitInfoPanel;
    public GameObject battlePreview;
    public GameObject tileInfoPanel;
    public GameObject weaponInfoPanel;
    public GameObject healthPanel;
    public GameObject unitWindow;
    public GameObject unitDetailsWindow;
    public GameObject resultScreen;

    public List<GameObject> UI_Prefabs;             //TBD

    private Random.State currentSeed;

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
        dijsktrasFullMap = new Dijsktras(mapTileGraph);
        actionMenu = GameObject.Find("ActionMenu");
        unitInfoPanel = GameObject.Find("UnitInfoCanvas");
        battlePreview = GameObject.Find("BattlePreviewCanvas");
        tileInfoPanel = GameObject.Find("TileInfoCanvas");
        weaponInfoPanel = GameObject.Find("WeaponInfoCanvas");
        healthPanel = GameObject.Find("HealthCanvas");
        unitWindow = GameObject.Find("UnitWindow");
        unitDetailsWindow = GameObject.Find("UnitDetailsWindow");
        resultScreen = GameObject.Find("ResultScreen");
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

    public int GetNextRandomNumber()
    {
        int nextRand = UnityEngine.Random.Range(0, 100);
        currentSeed = UnityEngine.Random.state;
        return nextRand;
    }
}
