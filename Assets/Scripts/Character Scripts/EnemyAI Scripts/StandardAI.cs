using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardAI : MonoBehaviour
{
    private CharacterInfo characterInfo;

    public CharacterInfo chosenTarget;
    public int chosenWeapon;
    public TileInfo chosenTile;

    public ENEMY_ACTIONS chosenAction;

    void Start()
    {
        characterInfo = gameObject.GetComponent<CharacterInfo>();
    }

    public void GetTarget()
    {
        FindTargets();

        if(characterInfo.targetList.Count > 0)
        {
            ChooseTarget();
            chosenTile = characterInfo.FindBestTile(characterInfo.weaponList[chosenWeapon], chosenTarget);
            chosenAction = ENEMY_ACTIONS.ATTACK;
        }
        else
        {
            chosenTarget = FindNearestEnemy();
            chosenAction = ENEMY_ACTIONS.MOVE;
        }
    }

    private void FindTargets()
    {
        characterInfo.GetTargetList();
        characterInfo.RunDijsktras();
    }

    private void ChooseTarget()
    {
        int currentBestScore = 0;

        foreach(var target in characterInfo.targetList)
        {
            int i = 0;
            while(characterInfo.weaponList[i] != null)
            {
                int newScore = 0;
                newScore = EvaluateTarget(target, characterInfo.weaponList[i]);
                if(newScore > currentBestScore)
                {
                    currentBestScore = newScore;
                    chosenTarget = target;
                    chosenWeapon = i;
                }
                i++;
            }
        }
    }

    private int EvaluateTarget(CharacterInfo target, WeaponData weapon)
    {
        int targetScore = 0;

        targetScore = EvaluateDamage(target, weapon);

        if (targetScore < 0)
            targetScore = 0;

        if(weapon.MAXRANGE > target.weaponList[0].MAXRANGE)
        {
            targetScore = targetScore * 2;
        }

        return targetScore;
    }

    private int EvaluateDamage(CharacterInfo target, WeaponData weapon)
    {
        int damage = 0;
        if (weapon.isRanged)
        {
            switch (weapon.damageType)
            {
                case DAMAGE_TYPE.ENERGY:
                    damage = characterInfo.characterData.PREC + weapon.ATK - target.characterData.SHIELD;
                    break;
                case DAMAGE_TYPE.PHYSICAL:
                    damage = characterInfo.characterData.PREC + weapon.ATK - target.characterData.ARMOR;
                    break;

            }
        }
        else
        {
            switch (weapon.damageType)
            {
                case DAMAGE_TYPE.ENERGY:
                    damage = characterInfo.characterData.PHY + weapon.ATK - target.characterData.SHIELD;
                    break;
                case DAMAGE_TYPE.PHYSICAL:
                    damage = characterInfo.characterData.PHY + weapon.ATK - target.characterData.ARMOR;
                    break;
            }
        }

        return damage;
    }

    private void FindBestTile()
    {
        Dijsktras dijsktras = new Dijsktras(WorldStateInfo.Instance.mapTileGraph);
        List<Node> tilesToEvaluate = new List<Node>();
        Graph tilesInRange = new Graph();
        int range = characterInfo.weaponList[chosenWeapon].MAXRANGE;
        tilesInRange = WorldStateInfo.Instance.mapTileGraph.BuildSubGraph(chosenTarget.currentTile.index, 0, characterInfo.weaponList[chosenWeapon].MINRANGE, range, chosenTarget);

        foreach(var tile in tilesInRange.graphNodes)
        {
            Node nextTile = characterInfo.subGraph.graphNodes.Find(x => x.tile == tile.tile);
            nextTile.weight = tile.weight;
            if(nextTile != null)
            {
                tilesToEvaluate.Add(nextTile);
            }
        }

        while(chosenTile == null && tilesToEvaluate.Count > 0 && range >= characterInfo.weaponList[chosenWeapon].MINRANGE)
        {
            chosenTile = EvaluateTileAtRange(range, tilesToEvaluate);
            tilesToEvaluate.RemoveAll(x => x.weight == range);
            range = range - 1;
        }
    }

    private TileInfo EvaluateTileAtRange(int range, List<Node>tilesToEvaluate)
    {
        TileInfo chosenTile = null;
        foreach(var tile in tilesToEvaluate)
        {
            if(tile.weight == range && (!tile.tile.isOccupied || tile.tile.occupant == characterInfo))
            {
                if(chosenTile == null || (characterInfo.shortestPath.dist[tile.tile.index] < characterInfo.shortestPath.dist[chosenTile.index]))
                    chosenTile = tile.tile;
            }
        }
        return chosenTile;
    }

    private CharacterInfo FindNearestEnemy()
    {
        Dijsktras dijsktras = new Dijsktras(WorldStateInfo.Instance.mapTileGraph);
        List<CharacterInfo> playerCharacters = WorldStateInfo.Instance.battleController.GetPlayerUnits();
        CharacterInfo closestCharacter = new CharacterInfo();

        int closestTileIndex = int.MaxValue;

        dijsktras.DijsktrasAlogrithm(characterInfo.currentTile.index, characterInfo);

        foreach(var enemy in playerCharacters)
        {
            if(closestTileIndex == int.MaxValue)
            {
                closestTileIndex = enemy.currentTile.index;
                closestCharacter = enemy;
            }
            else
            {
                if(dijsktras.dist[closestTileIndex] < dijsktras.dist[enemy.currentTile.index])
                {
                    closestTileIndex = enemy.currentTile.index;
                    closestCharacter = enemy;
                }
            }
        }

        return closestCharacter;
    }
}
