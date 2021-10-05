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
    public AI_TYPE aiType;

    private List<CharacterInfo> validTargets;

    void Start()
    {
        characterInfo = gameObject.GetComponent<CharacterInfo>();
        validTargets = new List<CharacterInfo>();
    }

    public void GetTarget()
    {
        FindTargets();
       
        switch (aiType) 
        {
            case AI_TYPE.SEEKER:
                if (validTargets.Count > 0)
                {
                    ChooseTarget();
                    chosenTile = characterInfo.FindBestTile(characterInfo.weaponList[chosenWeapon], chosenTarget);
                    if (chosenTile != null)
                    {
                        chosenAction = ENEMY_ACTIONS.ATTACK;
                        characterInfo.ChangeWeapon(characterInfo.weaponList[chosenWeapon]);
                    }
                    else
                    {
                        chosenTile = FindClosestTile();
                        chosenAction = ENEMY_ACTIONS.MOVE;
                    }
                }
                else
                {
                    chosenTarget = FindNearestEnemy();
                    chosenTile = FindClosestTile();
                    chosenAction = ENEMY_ACTIONS.MOVE;
                }
                break;
            case AI_TYPE.STANDARD:
                if(characterInfo.targetList.Count > 0)
                {
                    ChooseTarget();
                    chosenTile = characterInfo.FindBestTile(characterInfo.weaponList[chosenWeapon], chosenTarget);
                    chosenAction = ENEMY_ACTIONS.ATTACK;
                }
                else
                {
                    chosenAction = ENEMY_ACTIONS.WAIT;
                }
                break;
            case AI_TYPE.STATIONARY:
                if(characterInfo.localTargets.Count > 0)
                {
                    ChooseTarget();
                    chosenAction = ENEMY_ACTIONS.ATTACK;
                }
                else
                {
                    chosenAction = ENEMY_ACTIONS.WAIT;
                }
                break;
        }
        
    }

    private void FindTargets()
    {
        characterInfo.GetTargetList();
        foreach(var target in characterInfo.targetList)
        {
            if (characterInfo.EvaluateIsEnemy(target))
            {
                validTargets.Add(target);
            }
        }
        characterInfo.RunDijsktras();
    }

    private void ChooseTarget()
    {
        int currentBestScore = 0;

        //foreach(var target in validTargets)
        //{
        //    if (target.characterData.characterType != CHARACTER_TYPE.ENEMY)
        //    {
        //        int i = 0;
        //        while (characterInfo.weaponList[i] != null)
        //        {
        //            int newScore = 0;
        //            Graph weaponSubgraph = WorldStateInfo.Instance.mapTileGraph.BuildSubGraph(characterInfo.currentTile.index, 0, characterInfo.weaponList[i].MINRANGE, characterInfo.weaponList[i].MAXRANGE, characterInfo);
        //            newScore = EvaluateTarget(target, characterInfo.weaponList[i]);
        //            if (newScore > currentBestScore)
        //            {
        //                currentBestScore = newScore;
        //                chosenTarget = target;
        //                chosenWeapon = i;
        //            }
        //            else if (newScore == currentBestScore)
        //            {
        //                chosenTarget = FindClosest(chosenTarget, target);
        //            }
        //            i++;
        //        }
        //    }
        //}
        int i = 0;
        while (characterInfo.weaponList[i])
        {
            List<CharacterInfo> weaponTargets = new List<CharacterInfo>();

            weaponTargets.AddRange(FindTargetsInWeaponSubGraph(i));

            foreach(var target in validTargets)
            {
                if(target.characterData.characterType != CHARACTER_TYPE.ENEMY && weaponTargets.Exists(x=>x == target))
                {
                    int newScore = 0;
                    newScore = EvaluateTarget(target, characterInfo.weaponList[i]);
                    if (newScore > currentBestScore)
                    {
                        currentBestScore = newScore;
                        chosenTarget = target;
                        chosenWeapon = i;
                    }
                    else if (newScore == currentBestScore)
                    {
                        chosenTarget = FindClosest(chosenTarget, target);
                    }
                }
            }

            i++;
        }
    }

    private int EvaluateTarget(CharacterInfo target, WeaponData weapon)
    {
        int targetScore = 0;
        //only evaluate if the target is in range of character with the weapon being evaluated
        if (!target.flagIsDead && characterInfo.shortestPath.tileDist[target.currentTile.index] <= characterInfo.GetMOVE() + weapon.MAXRANGE)
        {
            targetScore = EvaluateDamage(target, weapon);

            if (targetScore < 0)
                targetScore = 0;

            if (weapon.MAXRANGE > target.weaponList[0].MAXRANGE)
            {
                targetScore = targetScore * 2;
            }
        }
        else if (target.flagIsDead)
        {
            targetScore = int.MinValue;
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

    private IEnumerable<CharacterInfo> FindTargetsInWeaponSubGraph(int weaponIndex)
    {
        List<CharacterInfo> targets = new List<CharacterInfo>();
        Graph weaponSubgraph = WorldStateInfo.Instance.mapTileGraph.BuildSubGraph(characterInfo.currentTile.index, characterInfo.GetMOVE(), characterInfo.weaponList[weaponIndex].MINRANGE, characterInfo.weaponList[weaponIndex].MAXRANGE, characterInfo);

        foreach(var tile in weaponSubgraph.graphNodes)
        {
            if (tile.tile.isOccupied)
            {
                if (characterInfo.EvaluateIsEnemy(tile.tile.occupant) && (tile.colorType == COLOR_TYPE.MOVEMENT || tile.colorType == COLOR_TYPE.ATTACK))
                {
                    targets.Add(tile.tile.occupant);
                }
            }
        }

        return targets;
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

    private TileInfo FindClosestTile()
    {
        Dijsktras dijsktras = new Dijsktras(WorldStateInfo.Instance.mapTileGraph);

        dijsktras.DijsktrasAlogrithm(chosenTarget.currentTile.index, characterInfo);

        int currentTile = chosenTarget.currentTile.index;

        int currentDistance = dijsktras.dist[characterInfo.currentTile.index];

        Node nextTile = null;

        //while(dijsktras.parent[currentTile] != -1)
        //{
        //    currentTile = dijsktras.parent[currentTile];
        //    nextTile = characterInfo.subGraph.graphNodes.Find(x => x.nodeIndex == currentTile);
            

        //    if(nextTile != null && nextTile.colorType == COLOR_TYPE.MOVEMENT && !nextTile.tile.isOccupied)
        //    {
        //        return nextTile.tile;
        //    }
        //}

        foreach(var tile in characterInfo.subGraph.graphNodes)
        {
            if(tile.colorType == COLOR_TYPE.MOVEMENT && dijsktras.dist[tile.tile.index] < currentDistance && !tile.tile.isOccupied)
            {
                nextTile = tile;
                currentDistance = dijsktras.dist[tile.tile.index];
            }
        }

        if (nextTile == null)
            nextTile = characterInfo.subGraph.graphNodes[0];

        return nextTile.tile;
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

    private CharacterInfo FindClosest(CharacterInfo initialTarget, CharacterInfo compareTarget)
    {
        if (characterInfo.shortestPath.dist[initialTarget.currentTile.index] > characterInfo.shortestPath.dist[compareTarget.currentTile.index])
            return compareTarget;

        return initialTarget;
    }

    private CharacterInfo FindNearestEnemy()
    {
        Dijsktras dijsktras = new Dijsktras(WorldStateInfo.Instance.mapTileGraph);
        List<CharacterInfo> playerCharacters = WorldStateInfo.Instance.battleController.GetPlayerUnits();
        CharacterInfo closestCharacter = null;

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
                if(dijsktras.dist[closestTileIndex] > dijsktras.dist[enemy.currentTile.index])
                {
                    closestTileIndex = enemy.currentTile.index;
                    closestCharacter = enemy;
                }
            }
        }

        return closestCharacter;
    }
}
