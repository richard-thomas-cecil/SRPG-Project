using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnitWindowController : MonoBehaviour
{
    [SerializeField] private GameObject unitStatsPanelPrefab;
    [SerializeField] private GameObject unitNamePanelPrefab;

    private RectTransform unitNameStaticPosition;
    private RectTransform unitStatsStaticPosition;

    private List<GameObject> unitStatPanels = new List<GameObject>();
    private List<GameObject> unitNamePanels = new List<GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        unitNameStaticPosition = transform.Find("NameDescriptivePanel").gameObject.GetComponent<RectTransform>();
        unitStatsStaticPosition = transform.Find("StatNamesPanel").gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildWindow(List<CharacterInfo> units)
    {
        int unitsSet = 0;

        foreach(var unit in units)
        {
            GameObject unitNameInfo = Instantiate(unitNamePanelPrefab);
            GameObject unitStatsInfo = Instantiate(unitStatsPanelPrefab);

            unitNameInfo.transform.SetParent(this.gameObject.transform, false);
            unitStatsInfo.transform.SetParent(this.gameObject.transform, false);

            unitNameInfo.GetComponent<RectTransform>().anchoredPosition= new Vector3(unitNameStaticPosition.anchoredPosition.x, unitNameStaticPosition.anchoredPosition.y - (unitNameStaticPosition.sizeDelta.y * (unitsSet + 1)), 0);
            unitStatsInfo.GetComponent<RectTransform>().anchoredPosition = new Vector3(unitStatsStaticPosition.anchoredPosition.x, unitStatsStaticPosition.anchoredPosition.y - (unitStatsStaticPosition.sizeDelta.y * (unitsSet + 1)), 0);

            unitNameInfo.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = unit.characterData.CharacterName;
            unitNameInfo.transform.Find("CharacterSprite").GetComponent<Image>().sprite = unit.gameObject.GetComponent<SpriteRenderer>().sprite;

            unitStatsInfo.transform.Find("HP").GetComponent<TextMeshProUGUI>().text = unit.characterData.HP_CURRENT.ToString();
            unitStatsInfo.transform.Find("MAX HP").GetComponent<TextMeshProUGUI>().text = "/" + unit.characterData.HP.ToString();
            unitStatsInfo.transform.Find("PHY").GetComponent<TextMeshProUGUI>().text = unit.characterData.PHY.ToString();
            unitStatsInfo.transform.Find("PREC").GetComponent<TextMeshProUGUI>().text = unit.characterData.PREC.ToString();
            unitStatsInfo.transform.Find("AIM").GetComponent<TextMeshProUGUI>().text = unit.characterData.AIM.ToString();
            unitStatsInfo.transform.Find("ARMOR").GetComponent<TextMeshProUGUI>().text = unit.characterData.ARMOR.ToString();
            unitStatsInfo.transform.Find("SHIELD").GetComponent<TextMeshProUGUI>().text = unit.characterData.SHIELD.ToString();
            unitStatsInfo.transform.Find("SPEED").GetComponent<TextMeshProUGUI>().text = unit.characterData.SPEED.ToString();

            unitsSet = unitsSet + 1;

            unitStatPanels.Add(unitStatsInfo);
            unitNamePanels.Add(unitNameInfo);

        }
    }

    public void ShowWindow()
    {
        this.gameObject.SetActive(true);
    }

    public void DisableWindow()
    {
        this.gameObject.SetActive(false);
    }
}
