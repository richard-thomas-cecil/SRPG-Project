using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class CreateTiles
{
    [MenuItem("GameObject/Create Tile/Basic Tile", false, 0)]
    static void CreateTile()
    {
        GameObject basictile = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/TileInfo Prefabs/BasicTile.prefab", typeof(GameObject));
        GameObject newTile = (GameObject)PrefabUtility.InstantiatePrefab(basictile);

        string tileName = "BasicTile (" + Selection.activeTransform.childCount + ")";

        newTile.name = tileName;

        newTile.transform.SetParent(Selection.activeTransform);

        Selection.activeGameObject = newTile;
    }
}
