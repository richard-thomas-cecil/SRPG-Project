using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthPanelController : MonoBehaviour
{
    private GameObject attackerPanel;
    private GameObject defenderPanel;

    private TextMeshProUGUI attackerName;
    private TextMeshProUGUI attackerHealthText;
    private GameObject attackerHealthPositive;

    private TextMeshProUGUI defenderName;
    private TextMeshProUGUI defenderHealthText;
    private GameObject defenderHealthPositive;

    private GameObject damageBox;
    private TextMeshProUGUI damageText;

    private CharacterInfo attacker;
    private CharacterInfo defender;

    private RectTransform canvasRect;

    private Queue<int> healthUpdateQueue;
    private Queue<CharacterInfo> characterUpdateQueue;

    // Start is called before the first frame update
    void Start()
    {
        attackerPanel = transform.Find("AttackerPanel").gameObject;
        defenderPanel = transform.Find("DefenderPanel").gameObject;

        attackerName = attackerPanel.transform.Find("CharName").GetComponent<TextMeshProUGUI>();
        attackerHealthText = attackerPanel.transform.Find("HealthText").GetComponent<TextMeshProUGUI>();
        attackerHealthPositive = attackerPanel.transform.Find("HealthPositive").gameObject;

        defenderName = defenderPanel.transform.Find("CharName").GetComponent<TextMeshProUGUI>();
        defenderHealthText = defenderPanel.transform.Find("HealthText").GetComponent<TextMeshProUGUI>();
        defenderHealthPositive = defenderPanel.transform.Find("HealthPositive").gameObject;

        damageBox = transform.Find("DamageBox").gameObject;
        damageText = damageBox.transform.Find("DamageText").gameObject.GetComponent<TextMeshProUGUI>();

        canvasRect = this.gameObject.GetComponent<RectTransform>();

        healthUpdateQueue = new Queue<int>();
        characterUpdateQueue = new Queue<CharacterInfo>();

        attackerPanel.SetActive(false);
        defenderPanel.SetActive(false);
        damageBox.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPanel(CharacterInfo _attacker, CharacterInfo _defender)
    {
        attacker = _attacker;
        defender = _defender;

        attackerName.text = attacker.characterData.CharacterName;
        attackerHealthText.text = attacker.characterData.HP_CURRENT + "/" + attacker.characterData.HP;
        attackerHealthPositive.transform.localScale = new Vector3((float)((float)attacker.characterData.HP_CURRENT / (float)attacker.characterData.HP), attackerHealthPositive.transform.localScale.y, attackerHealthPositive.transform.localScale.z);

        defenderName.text = defender.characterData.CharacterName;
        defenderHealthText.text = defender.characterData.HP_CURRENT + "/" + defender.characterData.HP;
        defenderHealthPositive.transform.localScale = new Vector3((float)((float)defender.characterData.HP_CURRENT / (float)defender.characterData.HP), defenderHealthPositive.transform.localScale.y, defenderHealthPositive.transform.localScale.z);

        PositionPanels();

        attackerPanel.SetActive(true);
        defenderPanel.SetActive(true);
    }

    public void AddToUpdateQueue(int toAdd, CharacterInfo characterToAdd)
    {
       healthUpdateQueue.Enqueue(toAdd);
       characterUpdateQueue.Enqueue(characterToAdd);
    }

    public void UpdatePanel()
    {
        int toUpdate = healthUpdateQueue.Dequeue();
        CharacterInfo characterInfo = characterUpdateQueue.Dequeue();
        
        damageText.gameObject.GetComponent<Animator>().ResetTrigger("Play");

        if (characterInfo == attacker)
        {
            attackerName.text = attacker.characterData.CharacterName;
            attackerHealthText.text = attacker.GetHPCurrent() + "/" + attacker.characterData.HP;
            attackerHealthPositive.transform.localScale = new Vector3((float)((float)attacker.GetHPCurrent() / (float)attacker.characterData.HP), attackerHealthPositive.transform.localScale.y, attackerHealthPositive.transform.localScale.z);
            DamageTextMovement(toUpdate, characterInfo);
        }

        else if (characterInfo == defender)
        {
            defenderName.text = defender.characterData.CharacterName;
            defenderHealthText.text = defender.GetHPCurrent() + "/" + defender.characterData.HP;
            defenderHealthPositive.transform.localScale = new Vector3((float)((float)defender.GetHPCurrent() / (float)defender.characterData.HP), defenderHealthPositive.transform.localScale.y, defenderHealthPositive.transform.localScale.z);
            DamageTextMovement(toUpdate, characterInfo);
        }
    }

    private void DamageTextMovement(int damageValue, CharacterInfo defender)
    {
        Vector2 newDefenderPosition;
        Vector2 viewPortPosDefender = Camera.main.WorldToViewportPoint(defender.gameObject.transform.position);
        Vector2 unitSize = Camera.main.WorldToViewportPoint(new Vector2(1.1f, 1.1f));
        newDefenderPosition = new Vector2(((viewPortPosDefender.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)), ((viewPortPosDefender.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f) + ((unitSize.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f))));

        if (damageValue > 0)
            damageText.text = damageValue.ToString();
        else if (damageValue == 0)
            damageText.text = "No Damage";
        else
            damageText.text = "Miss";

        damageBox.gameObject.GetComponent<RectTransform>().localPosition = newDefenderPosition;
        damageBox.gameObject.SetActive(true);

        damageText.gameObject.GetComponent<Animator>().SetTrigger("Play");
    }

    private void PositionPanels()
    {
        Vector2 newAttackerPosition;
        Vector2 newDefenderPosition;
        Vector2 viewPortPosAttacker = Camera.main.WorldToViewportPoint(attacker.gameObject.transform.position);
        Vector2 viewPortPosDefender = Camera.main.WorldToViewportPoint(defender.gameObject.transform.position);
        Vector2 unitSize = Camera.main.WorldToViewportPoint(new Vector2(1.1f, 1.1f));
        newAttackerPosition = new Vector2(((viewPortPosAttacker.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)), ((viewPortPosAttacker.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f) - ((unitSize.y * canvasRect.sizeDelta.y)-(canvasRect.sizeDelta.y*0.5f))));
        newDefenderPosition = new Vector2(((viewPortPosDefender.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)), ((viewPortPosDefender.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f) - ((unitSize.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f))));

        Debug.Log(newDefenderPosition);

        attackerPanel.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(newAttackerPosition.x, newAttackerPosition.y);
        defenderPanel.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(newDefenderPosition.x, newDefenderPosition.y);
    }

    public void DisablePanel()
    {
        attackerPanel.SetActive(false);
        defenderPanel.SetActive(false);
    }
}
