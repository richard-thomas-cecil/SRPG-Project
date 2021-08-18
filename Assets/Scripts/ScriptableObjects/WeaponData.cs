using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "NewWeapon", menuName = "Scriptable Objects/New Weapon", order = 2)]
public class WeaponData : ScriptableObject
{
    public string weaponName;

    public DAMAGE_TYPE damageType;
    public bool isRanged;

    public int ATK;
    public int HIT;
    public int CRIT;
    public int AMMO;
    public int AMMO_CURRENT;
    public int MINRANGE;
    public int MAXRANGE;

    void Awake()
    {
        if(MAXRANGE > 1)
        {
            isRanged = true;
        }
    }
}
