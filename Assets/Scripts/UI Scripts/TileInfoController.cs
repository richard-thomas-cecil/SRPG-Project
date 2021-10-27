using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TileInfoController : MonoBehaviour
{
    public TileInfo currentTile;

    private GameObject tileInfoPanel;

    private RectTransform canvasRectTransform;

    private TextMeshProUGUI tileType;
    private TextMeshProUGUI defenseBonus;
    private TextMeshProUGUI avoidBonus;

    private float tilePanelPositionOffset = 144;

    // Start is called before the first frame update
    void Start()
    {
        tileInfoPanel = transform.Find("TileInfoPanel").gameObject;

        canvasRectTransform = GetComponent<RectTransform>();

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

        if(WorldStateInfo.Instance.player.currentTile.transform.position.x < WorldStateInfo.Instance.mainCamera.transform.position.x - 3.0f && WorldStateInfo.Instance.player.currentTile.transform.position.y < WorldStateInfo.Instance.mainCamera.transform.position.y - 1.0f)
        {
            tileInfoPanel.transform.position = new Vector3(canvasRectTransform.rect.width - tilePanelPositionOffset, tileInfoPanel.transform.position.y, tileInfoPanel.transform.position.z);
        }
        else
        {
            tileInfoPanel.transform.position = new Vector3(tilePanelPositionOffset, tileInfoPanel.transform.position.y, tileInfoPanel.transform.position.z);
        }

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
