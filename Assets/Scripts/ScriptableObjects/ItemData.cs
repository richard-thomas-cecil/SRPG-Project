using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Scriptable Objects/New Item", order = 3)]
public class ItemData : ScriptableObject
{
    public ITEM_TYPE type;
    public string itemName;

    public int INTENSITY;           //Generic value of item effect, no matter what it does
}
