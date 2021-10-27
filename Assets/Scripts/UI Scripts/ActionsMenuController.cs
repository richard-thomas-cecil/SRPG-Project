using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;



public class ActionsMenuController : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject buttonPool;
    public GameObject itemPanel;
    public GameObject targetPanel;
    public GameObject itemActionPanel;
    public ButtonInfo[] buttons;

    public Button confirmButton;
    public Button backButton;

    public List<CharacterInfo> targetList;

    public List<ACTION_BUTTON_LIST> currentActionList;      //Keeps track of current actions in the menu so they may be removed later

    Stack<GameObject> previousButtonsPressed;

    Navigation defaultMode = new Navigation();              //All buttons should default to navigation mode of none
    Navigation setVerticalNav = new Navigation();           //once added to list, set to vertical navigation   

    [SerializeField]private GameObject itemButtonPrefab;
    [SerializeField]private GameObject targetButtonPrefab;
    [SerializeField]private GameObject weaponButtonPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        //Intialize variables
        currentActionList = new List<ACTION_BUTTON_LIST>();
        targetList = new List<CharacterInfo>();
        previousButtonsPressed = new Stack<GameObject>();

        setVerticalNav.mode = Navigation.Mode.Vertical;
        defaultMode.mode = Navigation.Mode.None;

        menuPanel = transform.Find("MenuPanel").gameObject;
        buttonPool = transform.Find("ButtonPool").gameObject;
        itemPanel = transform.Find("ItemPanel").gameObject;
        targetPanel = transform.Find("TargetPanel").gameObject;
        itemActionPanel = transform.Find("ItemActionPanel").gameObject;

        



        menuPanel.SetActive(false);
        itemPanel.SetActive(false);
        itemActionPanel.SetActive(false);


        //foreach (var button in buttons)
        //{
        //    AddButtonToMenu(button.buttonID);
        //}

        //ResizeMenuPanel();
    }

    private void Start()
    {
        //InitialzeButtonFunctions();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableUI()
    {
        this.gameObject.SetActive(false);
    }


    public void EnableUI()
    {
        this.gameObject.SetActive(true);
    }

    public void InitialzeButtonFunctions()
    {
        buttons = new ButtonInfo[buttonPool.transform.childCount];

        //Intialize buttons array with all buttons in ButtonPool
        //Buttons have defined ID from ACTION_BUTTON_LIST enumeration
        //Button ID should be assigned in Inspector
        for (int i = 0; i < buttons.Length; i++)
        {
            ButtonInfo newButton = buttonPool.transform.GetChild(i).GetComponent<ButtonInfo>();
            buttons[(int)newButton.buttonID] = newButton;
        }

        foreach (var button in buttons)
        {
            Debug.Log(button.name);
            switch (button.buttonID)
            {
                case ACTION_BUTTON_LIST.WAIT:
                    button.button.onClick.AddListener(WorldStateInfo.Instance.battleController.WaitAction);
                    break;
                case ACTION_BUTTON_LIST.ATTACK:
                    button.button.onClick.AddListener(WorldStateInfo.Instance.battleController.AttackAction);
                    break;
                case ACTION_BUTTON_LIST.INVENTORY:
                    button.button.onClick.AddListener(WorldStateInfo.Instance.battleController.InventoryAction);
                    break;
                case ACTION_BUTTON_LIST.UNITS:
                    button.button.onClick.AddListener(WorldStateInfo.Instance.battleController.UnitsAction);
                    break;
                case ACTION_BUTTON_LIST.OBJECTIVE:
                    break;
                case ACTION_BUTTON_LIST.OPTIONS:
                    break;
                case ACTION_BUTTON_LIST.SUSPEND:
                    break;
                case ACTION_BUTTON_LIST.END_TURN:
                    button.button.onClick.AddListener(WorldStateInfo.Instance.battleController.EndTurnAction);
                    break;
            }
        }

        backButton = itemActionPanel.transform.Find("BackButton").gameObject.GetComponent<Button>();
        backButton.onClick.AddListener(ReverseMenu);
        confirmButton = itemActionPanel.transform.Find("ConfirmButton").GetComponent<Button>();
        confirmButton.gameObject.SetActive(false);
    }

    //Create the Action menu by passing in a list of Action IDs from Battle Controller
    //Calls AddButtonToMenu to add button via actionID, then resizes the action panel to fit all actions
    public void BuildActionMenu(List<ACTION_BUTTON_LIST> actions)
    {
        foreach(var action in actions)
        {
            AddButtonToMenu(action);
        }

        SetPanelNavigation(menuPanel);

        ResizeMenuPanel(menuPanel);

        EventSystem.current.SetSelectedGameObject(buttons[(int)actions[0]].gameObject);


        menuPanel.SetActive(true);
    }

    public void ResetMenus()
    {
        previousButtonsPressed.Clear();
        ResetActionMenu();
        ResetMenuPanel(itemPanel);
        ResetMenuPanel(targetPanel);
        confirmButton.gameObject.SetActive(false);
        ResetMenuPanel(itemActionPanel);
    }

    //Go through current assembled actions in list and reset everything to default state
    //Should be called at the end of each action function
    public void ResetActionMenu()
    {
        menuPanel.SetActive(false);

        foreach (var action in currentActionList)
        {
            buttons[(int)action].button.navigation = defaultMode;
            buttons[(int)action].transform.SetParent(buttonPool.transform);
            buttons[(int)action].gameObject.SetActive(false);
        }

        ResizeMenuPanel(menuPanel);

        EventSystem.current.SetSelectedGameObject(null);

        menuPanel.SetActive(false);

        currentActionList.Clear();
    }

    public void ResetMenuPanel(GameObject panel)
    {
        for (int i = 0; i < panel.transform.childCount; i++)
        {
            if(!(panel == itemActionPanel && (panel.transform.GetChild(i).gameObject == confirmButton.gameObject || panel.transform.GetChild(i).gameObject == backButton.gameObject)))
                Object.Destroy(panel.transform.GetChild(i).gameObject);
        }

        ResizeMenuPanel(panel);
        panel.SetActive(false);
    }

    //Resizes action menu based off of number of actions
    private void ResizeMenuPanel(GameObject panel)
    {
        float newHeight = 0f;

        for(int i = 0; i < panel.transform.childCount; i++)
        {
            newHeight = newHeight + panel.transform.GetChild(i).GetComponent<RectTransform>().rect.height;
        }

        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(panel.GetComponent<RectTransform>().rect.width, newHeight);
    }

    private void SetPanelNavigation(GameObject panel)
    {
        for (int i = 0; i < panel.transform.childCount; i++)
        {
            Navigation navigation = new Navigation();
            navigation.mode = Navigation.Mode.Explicit;
            if (i > 0)
            {
                navigation.selectOnUp = panel.transform.GetChild(i - 1).GetComponent<Button>();
            }
            if (i < panel.transform.childCount - 1)
            {
                navigation.selectOnDown = panel.transform.GetChild(i + 1).GetComponent<Button>();
            }
            if(i == 0)
            {
                navigation.selectOnUp = panel.transform.GetChild(panel.transform.childCount - 1).GetComponent<Button>();
            }
            if(i == panel.transform.childCount - 1)
            {
                navigation.selectOnDown = panel.transform.GetChild(0).GetComponent<Button>();
            }
            panel.transform.GetChild(i).GetComponent<Button>().navigation = navigation;
        }
    }

    //use action ID to add a button to the action menu as well as currentActionList to keep track of actions in list for later removal
    private void AddButtonToMenu(ACTION_BUTTON_LIST action)
    {
        buttons[(int)action].gameObject.SetActive(true);
        buttons[(int)action].transform.SetParent(menuPanel.transform, false);
        currentActionList.Add(action);
        //button.transform.position = new Vector2(button.transform.position.x, button.transform.position.y + (button.GetComponent<RectTransform>().rect.height * menuPanel.transform.childCount));
    }

    #region ITEM_PANEL

    public void CreateItemPanel(WeaponData[] weapons, ItemData[] items, ACTION_BUTTON_LIST action, CharacterInfo selectedCharacter)
    {
        int i = 0;
        while(weapons[i] != null)
        {
            if (((weapons[i].damageType != DAMAGE_TYPE.SUPPORT && selectedCharacter.canUseAttackWeapon) || (weapons[i].damageType == DAMAGE_TYPE.SUPPORT && selectedCharacter.canUseSupportWeapon)) || action == ACTION_BUTTON_LIST.INVENTORY)
            {
                GameObject newWeapon = Instantiate(weaponButtonPrefab);
                Button weaponButton = newWeapon.GetComponent<Button>();

                newWeapon.name = weapons[i].weaponName;

                newWeapon.GetComponentInChildren<TextMeshProUGUI>().text = weapons[i].weaponName;

                newWeapon.GetComponent<WeaponButtonInfo>().SetWeapon(weapons[i]);

                newWeapon.transform.SetParent(itemPanel.transform, false);

                newWeapon.GetComponent<RectTransform>().sizeDelta = new Vector2(itemPanel.GetComponent<RectTransform>().rect.width, newWeapon.GetComponent<RectTransform>().rect.height);

                SetWeaponButtonListener(weapons[i], weaponButton, action);
            }
            i++;
        }

        i = 0;

        if(action == ACTION_BUTTON_LIST.INVENTORY)
        {
            while(items[i] != null)
            {
                GameObject newItem = Instantiate(itemButtonPrefab);
                Button itemButton = newItem.GetComponent<Button>();

                newItem.name = items[i].itemName;

                newItem.GetComponentInChildren<TextMeshProUGUI>().text = items[i].itemName;

                newItem.transform.SetParent(itemPanel.transform, false);

                i++;
            }
        }
        SetPanelNavigation(itemPanel);

        ResizeMenuPanel(itemPanel);

        itemPanel.SetActive(true);

        if(action != ACTION_BUTTON_LIST.ATTACK_AND_MOVE)
            previousButtonsPressed.Push(EventSystem.current.currentSelectedGameObject);

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(itemPanel.transform.GetChild(0).gameObject);
    }

    public void RebuildItemMenu(WeaponData[] weapons, ItemData[] items, ACTION_BUTTON_LIST action, CharacterInfo selectedCharacter)
    {
        for(int j = 0; j < itemPanel.transform.childCount; j++)
        {           
            Object.Destroy(itemPanel.transform.GetChild(j).gameObject);
            
        }

        itemPanel.transform.DetachChildren();

        int i = 0;
        while (weapons[i] != null)
        {
            if (((weapons[i].damageType != DAMAGE_TYPE.SUPPORT && selectedCharacter.canUseAttackWeapon) || (weapons[i].damageType == DAMAGE_TYPE.SUPPORT && selectedCharacter.canUseSupportWeapon)) || action == ACTION_BUTTON_LIST.INVENTORY)
            {
                GameObject newWeapon = Instantiate(weaponButtonPrefab);
                Button weaponButton = newWeapon.GetComponent<Button>();

                newWeapon.name = weapons[i].weaponName;

                newWeapon.GetComponentInChildren<TextMeshProUGUI>().text = weapons[i].weaponName;

                newWeapon.GetComponent<WeaponButtonInfo>().SetWeapon(weapons[i]);

                newWeapon.transform.SetParent(itemPanel.transform, false);

                newWeapon.GetComponent<RectTransform>().sizeDelta = new Vector2(itemPanel.GetComponent<RectTransform>().rect.width, newWeapon.GetComponent<RectTransform>().rect.height);

                SetWeaponButtonListener(weapons[i], weaponButton, action);
            }
            i++;
        }

        i = 0;

        if (action == ACTION_BUTTON_LIST.INVENTORY)
        {
            while (items[i] != null)
            {
                GameObject newItem = Instantiate(itemButtonPrefab);
                Button itemButton = newItem.GetComponent<Button>();

                newItem.name = items[i].itemName;

                newItem.GetComponentInChildren<TextMeshProUGUI>().text = items[i].itemName;

                newItem.transform.SetParent(itemPanel.transform, false);

                i++;
            }
        }
        SetPanelNavigation(itemPanel);

        ResizeMenuPanel(itemPanel);

        itemPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(itemPanel.transform.GetChild(0).gameObject);
    }

    private void SetWeaponButtonListener(WeaponData weaponData, Button weaponButton, ACTION_BUTTON_LIST action)
    {
        if (action == ACTION_BUTTON_LIST.ATTACK)
            weaponButton.onClick.AddListener(delegate { WorldStateInfo.Instance.battleController.SelectTarget(weaponData); });
        else if (action == ACTION_BUTTON_LIST.INVENTORY)
        {
            weaponButton.onClick.AddListener(delegate { SetItemActionPanel(weaponData); });
        }
        return;
    }

    private void SetItemActionPanel(WeaponData weapon)
    {
        confirmButton.gameObject.SetActive(false);
        GameObject equipButton = GameObject.Instantiate(itemButtonPrefab);
        Button equip = equipButton.GetComponent<Button>();

        equipButton.transform.SetParent(itemActionPanel.transform, false);
        equipButton.transform.SetAsFirstSibling();

        equipButton.transform.Find("ActionText").gameObject.GetComponent<TextMeshProUGUI>().text = "Equip";

        equipButton.GetComponent<RectTransform>().sizeDelta = new Vector2(itemActionPanel.GetComponent<RectTransform>().rect.width, equipButton.GetComponent<RectTransform>().rect.height);

        equip.onClick.AddListener(delegate { WorldStateInfo.Instance.battleController.EquipWeapon(weapon); });
        itemActionPanel.SetActive(true);

        Navigation navigation = new Navigation();
        navigation.mode = Navigation.Mode.Explicit;

        navigation.selectOnDown = backButton;
        navigation.selectOnUp = backButton;

        equip.navigation = navigation;

        navigation.selectOnDown = equip;
        navigation.selectOnUp = equip;

        backButton.navigation = navigation;

        ResizeMenuPanel(itemActionPanel);
        //SetPanelNavigation(itemActionPanel);

        previousButtonsPressed.Push(EventSystem.current.currentSelectedGameObject);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(itemActionPanel.transform.GetChild(0).gameObject);
    }

    private void SetItemActionPanel(ItemData item)
    {

    }

    public void CreateTargetList(List<CharacterInfo> targets, WeaponData selectedWeapon, CharacterInfo selectedCharacter)
    {
        foreach(var target in targets)
        {
            if (selectedWeapon.damageType != DAMAGE_TYPE.SUPPORT && selectedCharacter.EvaluateIsEnemy(target))
            {
                GameObject newTarget = Instantiate(targetButtonPrefab);
                Button targetButton = newTarget.GetComponent<Button>();

                newTarget.GetComponent<TargetButtonInfo>().SetButtonInfo(target, selectedWeapon);


                targetButton.onClick.AddListener(delegate { WorldStateInfo.Instance.battleController.ProcessAttack(target, selectedWeapon); });

                newTarget.transform.SetParent(targetPanel.transform, false);
            }
            else if(selectedWeapon.damageType == DAMAGE_TYPE.SUPPORT && !selectedCharacter.EvaluateIsEnemy(target))
            {
                GameObject newTarget = Instantiate(targetButtonPrefab);
                Button targetButton = newTarget.GetComponent<Button>();

                newTarget.GetComponent<TargetButtonInfo>().SetButtonInfo(target, selectedWeapon);

                targetButton.onClick.AddListener(delegate { WorldStateInfo.Instance.battleController.ProcessHeal(target, selectedWeapon); });

                newTarget.transform.SetParent(targetPanel.transform, false);
            }
        }

        SetPanelNavigation(targetPanel);

        ResizeMenuPanel(targetPanel);

        targetPanel.SetActive(true);

        previousButtonsPressed.Push(EventSystem.current.currentSelectedGameObject);

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(targetPanel.transform.GetChild(0).gameObject);
    }

    public void AttackAndMovePanel(WeaponData[] weapons, CharacterInfo selectedTarget)
    {
        int i = 0;
        while (weapons[i] != null)
        {
            GameObject newWeapon = Instantiate(itemButtonPrefab);
            Button weaponButton = newWeapon.GetComponent<Button>();

            newWeapon.name = weapons[i].weaponName;

            newWeapon.GetComponentInChildren<TextMeshProUGUI>().text = weapons[i].weaponName;


            newWeapon.transform.SetParent(itemPanel.transform, false);

            newWeapon.GetComponent<RectTransform>().sizeDelta = new Vector2(itemPanel.GetComponent<RectTransform>().rect.width, newWeapon.GetComponent<RectTransform>().rect.height);

            SetAttackAndMoveListener(weapons[i], weaponButton, selectedTarget);

            i++;
        }

        SetPanelNavigation(itemPanel);

        ResizeMenuPanel(itemPanel);

        itemPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(itemPanel.transform.GetChild(0).gameObject);
    }

    private void SetAttackAndMoveListener(WeaponData weapon, Button weaponButton, CharacterInfo selectedTarget)
    {
        weaponButton.onClick.AddListener(delegate { WorldStateInfo.Instance.battleController.MoveToAttackPosition(weapon, selectedTarget); } );
    }

    public void ConfirmAttack(CharacterInfo target, WeaponData weapon)
    {
        itemActionPanel.SetActive(true);
        confirmButton.gameObject.SetActive(true);
        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(delegate { WorldStateInfo.Instance.battleController.ProcessAttack(target, weapon); });

        previousButtonsPressed.Push(EventSystem.current.currentSelectedGameObject);

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(itemActionPanel.transform.GetChild(0).gameObject);
    }
    #endregion

    #region REVERSE_MENU_TRAVERSAL
    public void ReverseMenu()
    {
        GameObject currentMenu = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        EventSystem.current.SetSelectedGameObject(null);
        if(currentMenu == menuPanel || previousButtonsPressed.Count == 0)
        {
            WorldStateInfo.Instance.battleController.CancelActionMenu();
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(previousButtonsPressed.Pop());
            //if(EventSystem.current.gameObject == targetPanel)
            //{

            //}
            ResetMenuPanel(currentMenu);
            if (WorldStateInfo.Instance.battleController.playerCursor.transform.position != WorldStateInfo.Instance.battleController.playerCursor.currentTile.transform.position)
            {
                WorldStateInfo.Instance.battleController.playerCursor.MoveCursor(WorldStateInfo.Instance.battleController.playerCursor.currentTile.transform.position, 5.0f);
            }
            if (WorldStateInfo.Instance.battlePreview.activeInHierarchy)
            {
                WorldStateInfo.Instance.battleController.ResetPreviewPanel();
            }
        }
    }
    #endregion
}
