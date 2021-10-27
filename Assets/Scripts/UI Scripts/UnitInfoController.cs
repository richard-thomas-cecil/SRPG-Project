using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitInfoController : MonoBehaviour
{
    private GameObject unitInfoPanel;
    private GameObject healthNegative;
    private GameObject healthPositive;

    private TextMeshProUGUI nameText;
    private TextMeshProUGUI healthText;

    // Start is called before the first frame update
    void Awake()
    {
        unitInfoPanel = transform.Find("UnitInfoPanel").gameObject;
        healthNegative = unitInfoPanel.transform.Find("HealthNegative").gameObject;
        healthPositive = unitInfoPanel.transform.Find("HealthPositive").gameObject;

        nameText = unitInfoPanel.transform.Find("NameTextBox").GetComponent<TextMeshProUGUI>();
        healthText = unitInfoPanel.transform.Find("HealthTextBox").GetComponent<TextMeshProUGUI>();

        unitInfoPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPanel(CharacterInfo characterInfo)
    {
        nameText.text = characterInfo.characterData.CharacterName;
        healthText.text = characterInfo.characterData.HP_CURRENT + "/" + characterInfo.characterData.HP;

        healthPositive.transform.localScale = new Vector3((float)characterInfo.characterData.HP_CURRENT / (float)characterInfo.characterData.HP, 1, 1);

        unitInfoPanel.SetActive(true);
    }

    public void DisablePanel()
    {
        nameText.text = "";
        healthText.text = "";

        healthPositive.transform.localScale = new Vector3(1f, 1f, 1f);

        unitInfoPanel.SetActive(false);
    }
}
