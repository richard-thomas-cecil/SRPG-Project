using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class CreateTiles
{
    [MenuItem("GameObject/Create Tile/Basic Tile %#t", false, 0)]
    static void CreateTile()
    {
        GameObject basictile = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/TileInfo Prefabs/BasicTile.prefab", typeof(GameObject));
        GameObject newTile = (GameObject)PrefabUtility.InstantiatePrefab(basictile);

        GameObject mapInfo = GameObject.Find("MapInfo");
        if(mapInfo == null)
        {
            mapInfo = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/MapInfo/MapInfo.prefab", typeof(GameObject));
            mapInfo = GameObject.Instantiate(mapInfo);
        }

        string tileName = "BasicTile (" + mapInfo.transform.childCount + ")";

        newTile.name = tileName;

        newTile.transform.SetParent(mapInfo.transform);

        Selection.activeGameObject = newTile;
    }
}
