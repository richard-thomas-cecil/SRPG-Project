using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInfo : MonoBehaviour
{
    public ACTION_BUTTON_LIST buttonID;
    public Button button;

    // Start is called before the first frame update
    void Awake()
    {
        button = this.GetComponent<Button> ();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
