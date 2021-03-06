using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TileInfoController : MonoBehaviour
{
    public TileInfo currentTile;

    private GameObject tileInfoPanel;

    private TextMeshProUGUI tileType;
    private TextMeshProUGUI defenseBonus;
    private TextMeshProUGUI avoidBonus;

    // Start is called before the first frame update
    void Start()
    {
        tileInfoPanel = transform.Find("TileInfoPanel").gameObject;

        tileType = tileInfoPanel.transform.Find("TileType").GetComponent<TextMeshProUGUI>();
        defenseBonus = tileInfoPanel.transform.Find("DefenseBonus").GetComponent<TextMeshProUGUI>();
        avoidBonus = tileInfoPanel.transform.Find("AvoidBonus").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPanel(TileInfo _currentTile)
    {
        currentTile = _currentTile;

        tileType.text = currentTile.tileName;
        defenseBonus.text = "Defense: " + currentTile.defBoost.ToString();
        avoidBonus.text = "Dodge: " + currentTile.dodgeBoost.ToString();

        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
    }

    public void DisablePanel()
    {
        gameObject.SetActive(false);
    }
}
