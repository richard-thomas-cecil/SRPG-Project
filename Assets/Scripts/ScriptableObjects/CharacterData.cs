using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Scriptable Objects/New Character", order = 2)]
public class CharacterData : ScriptableObject
{
    public CHARACTER_TYPE characterType;
    public string CharacterName;

    public int HP;          //Hit Points
    public int HP_CURRENT;  //Hit Points Current
    public int PHY;         //Melee attack strength
    public int PREC;        //Precision; ranged attack strength
    public int AIM;         //Aim; used in hit percent calculations
    public int ARMOR;       //Defense against non-energy weapons
    public int SHIELD;      //Defense agains energy weapons
    public int SPEED;       //Attack speed
    public int MOVE;        //Movement 

    [SerializeField]public int baseHit;
    [SerializeField]public int baseDodge;
    [SerializeField] public int baseCrit;

    #region DERIVED_STATS
    public int HIT;
    public int DODGE;
    public int CRIT;
    #endregion

    public int minRange;
    public int maxRange;
    public int supportRangeMin;
    public int supportRangeMax;

}
