using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseUIController : MonoBehaviour
{
    private FastTravelController fastTravelPanel;
    [SerializeField] private bool menuUp = true;

    // Start is called before the first frame update
    void Start()
    {
        fastTravelPanel = gameObject.transform.Find("FastTravelPanel").GetComponent<FastTravelController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableFastTravelPanel()
    {
        fastTravelPanel.EnablePanel();
        menuUp = true;
    }

    public void DisableMenues()
    {
        if(menuUp == true)
        {   
            fastTravelPanel.gameObject.SetActive(false);

            EventSystem.current.SetSelectedGameObject(null);
            menuUp = false;

            WorldStateInfo.Instance.player.SwitchControlType("BaseMap");
        }
    }
}
