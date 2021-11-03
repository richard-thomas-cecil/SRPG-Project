using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FastTravelController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnablePanel()
    {
        gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(transform.GetChild(0).gameObject);
    }

    public void DisablePanel()
    {
        gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
