using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnitDetailsWindowController : MonoBehaviour
{
    [SerializeField] private GameObject weaponStatsPanelPrefab;

    private RectTransform weaponStatsStaticPosition;

    private GameObject unitDetailsPanel;
    private GameObject weaponsPanel;

    private List<GameObject> weaponStatsPanel;

    // Start is called before the first frame update
    void Start()
    {
        unitDetailsPanel = transform.Find("UnitDetailsPanel").gameObject;
        weaponsPanel = unitDetailsPanel.transform.Find("WeaponsPanel").gameObject;
        weaponStatsStaticPosition = weaponsPanel.transform.Find("WeaponsTagPanel").gameObject.GetComponent<RectTransform>();
        weaponStatsPanel = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildPanel(CharacterInfo unit)
    {
        foreach(var weapon in weaponStatsPanel)
        {
            Object.Destroy(weapon);
        }

        weaponStatsPanel.Clear();

        this.gameObject.SetActive(true);

        transform.Find("CharacterSprite").GetComponent<Image>().sprite = unit.gameObject.GetComponent<SpriteRenderer>().sprite;

        unitDetailsPanel.transform.Find("HP_Value").GetComponent<TextMeshProUGUI>().text = unit.characterData.HP_CURRENT + "/" + unit.characterData.HP;
        unitDetailsPanel.transform.Find("PHY_Value").GetComponent<TextMeshProUGUI>().text = unit.characterData.PHY.ToString();
        unitDetailsPanel.transform.Find("PREC_Value").GetComponent<TextMeshProUGUI>().text = unit.characterData.PREC.ToString();
        unitDetailsPanel.transform.Find("AIM_Value").GetComponent<TextMeshProUGUI>().text = unit.characterData.AIM.ToString();
        unitDetailsPanel.transform.Find("ARMOR_Value").GetComponent<TextMeshProUGUI>().text = unit.characterData.ARMOR.ToString();
        unitDetailsPanel.transform.Find("SHIELD_Value").GetComponent<TextMeshProUGUI>().text = unit.characterData.SHIELD.ToString();
        unitDetailsPanel.transform.Find("SPEED_Value").GetComponent<TextMeshProUGUI>().text = unit.characterData.SPEED.ToString();
        unitDetailsPanel.transform.Find("MOVE_Value").GetComponent<TextMeshProUGUI>().text = unit.characterData.MOVE.ToString();


        int i = 0;

        while(unit.weaponList[i] != null)
        {
            GameObject newWeaponPanel = Instantiate(weaponStatsPanelPrefab);

            newWeaponPanel.transform.SetParent(weaponsPanel.transform, false);

            newWeaponPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(weaponStatsStaticPosition.anchoredPosition.x, weaponStatsStaticPosition.anchoredPosition.y - (weaponStatsStaticPosition.sizeDelta.y * (i + 1)), 0);

            newWeaponPanel.transform.Find("WeaponName").GetComponent<TextMeshProUGUI>().text = unit.weaponList[i].weaponName;
            newWeaponPanel.transform.Find("WeaponATK").GetComponent<TextMeshProUGUI>().text = unit.weaponList[i].ATK.ToString();
            newWeaponPanel.transform.Find("WeaponHIT").GetComponent<TextMeshProUGUI>().text = unit.weaponList[i].HIT.ToString();
            newWeaponPanel.transform.Find("WeaponCRIT").GetComponent<TextMeshProUGUI>().text = unit.weaponList[i].CRIT.ToString();
            newWeaponPanel.transform.Find("WeaponRange").GetComponent<TextMeshProUGUI>().text = unit.weaponList[i].MINRANGE + "-" + unit.weaponList[i].MAXRANGE;

            weaponStatsPanel.Add(newWeaponPanel);

            i++;
        }

        
    }

    public void DisablePanel()
    {
        this.gameObject.SetActive(false);

        foreach(var weapon in weaponStatsPanel)
        {
            Object.Destroy(weapon);
        }
        weaponStatsPanel.Clear();
    }
}
