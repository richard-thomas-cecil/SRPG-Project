using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TargetButtonInfo : MonoBehaviour, ISelectHandler
{
    public CharacterInfo targetInfo;
    private WeaponData weapon;


    public void OnSelect(BaseEventData eventData)
    {
        WorldStateInfo.Instance.battleController.playerCursor.MoveCursor(targetInfo.transform.position, 1.0f);
        WorldStateInfo.Instance.battleController.ConfirmAttack(targetInfo, weapon, false);
    }

    public void SetButtonInfo(CharacterInfo character, WeaponData _weapon)
    {
        targetInfo = character;
        weapon = _weapon;
    }
}
