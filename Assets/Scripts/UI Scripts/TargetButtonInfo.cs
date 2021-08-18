using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TargetButtonInfo : MonoBehaviour, ISelectHandler
{
    public CharacterInfo targetInfo;

    public void OnSelect(BaseEventData eventData)
    {
        WorldStateInfo.Instance.battleController.playerCursor.MoveCursor(targetInfo.transform.position, 1.0f);
    }
}
