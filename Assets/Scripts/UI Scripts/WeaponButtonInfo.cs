using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponButtonInfo : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private WeaponData weapon;


    public void OnSelect(BaseEventData eventData)
    {
        WorldStateInfo.Instance.battleController.SetWeaponInfoPanel(weapon, eventData.selectedObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetWeapon(WeaponData _weapon)
    {
        weapon = _weapon;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        WorldStateInfo.Instance.battleController.DisableWeaponInfoPanel();
    }
}
