using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class ActionsMenuController : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject buttonPool;
    public ButtonInfo[] buttons;

    public List<ACTION_BUTTON_LIST> currentActionList;      //Keeps track of current actions in the menu so they may be removed later

    Navigation defaultMode = new Navigation();              //All buttons should default to navigation mode of none
    Navigation setVerticalNav = new Navigation();           //once added to list, set to vertical navigation   

    // Start is called before the first frame update
    void Start()
    {
        //Intialize variables
        currentActionList = new List<ACTION_BUTTON_LIST>();

        setVerticalNav.mode = Navigation.Mode.Vertical;
        defaultMode.mode = Navigation.Mode.None;

        menuPanel = transform.Find("MenuPanel").gameObject;
        buttonPool = transform.Find("ButtonPool").gameObject;

        buttons = new ButtonInfo[buttonPool.transform.childCount];

        //Intialize buttons array with all buttons in ButtonPool
        //Buttons have defined ID from ACTION_BUTTON_LIST enumeration
        //Button ID should be assigned in Inspector
        for (int i = 0; i < buttons.Length; i++)
        {
            ButtonInfo newButton = buttonPool.transform.GetChild(i).GetComponent<ButtonInfo>();
            buttons[(int)newButton.buttonID] = newButton;
        }


        //foreach (var button in buttons)
        //{
        //    AddButtonToMenu(button.buttonID);
        //}

        //ResizeMenuPanel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Create the Action menu by passing in a list of Action IDs from Battle Controller
    //Calls AddButtonToMenu to add button via actionID, then resizes the action panel to fit all actions
    public void BuildActionMenu(List<ACTION_BUTTON_LIST> actions)
    {
        foreach(var action in actions)
        {
            AddButtonToMenu(action);
        }

        ResizeMenuPanel();

        EventSystem.current.SetSelectedGameObject(buttons[(int)actions[0]].gameObject);

        menuPanel.SetActive(true);
    }


    //Go through current assembled actions in list and reset everything to default state
    //Should be called at the end of each action function
    public void ResetActionMenu()
    {
        foreach(var action in currentActionList)
        {
            buttons[(int)action].button.navigation = defaultMode;
            buttons[(int)action].transform.SetParent(buttonPool.transform);
        }

        ResizeMenuPanel();

        EventSystem.current.SetSelectedGameObject(null);

        menuPanel.SetActive(false);

        currentActionList.Clear();
    }

    //Resizes action menu based off of number of actions
    private void ResizeMenuPanel()
    {
        float newHeight = 0f;

        for(int i = 0; i < menuPanel.transform.childCount; i++)
        {
            newHeight = newHeight + menuPanel.transform.GetChild(i).GetComponent<RectTransform>().rect.height;
        }

        menuPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(menuPanel.GetComponent<RectTransform>().rect.width, newHeight);
    }

    //use action ID to add a button to the action menu as well as currentActionList to keep track of actions in list for later removal
    private void AddButtonToMenu(ACTION_BUTTON_LIST action)
    {  
        buttons[(int)action].button.navigation = setVerticalNav;
        buttons[(int)action].transform.SetParent(menuPanel.transform, false);
        currentActionList.Add(action);
        //button.transform.position = new Vector2(button.transform.position.x, button.transform.position.y + (button.GetComponent<RectTransform>().rect.height * menuPanel.transform.childCount));
    }

    
}
