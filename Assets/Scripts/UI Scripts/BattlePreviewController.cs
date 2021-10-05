using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattlePreviewController : MonoBehaviour
{
    GameObject previewPanel;

    TextMeshProUGUI playerName;
    TextMeshProUGUI playerHP;
    TextMeshProUGUI playerDamage;
    TextMeshProUGUI playerHit;
    TextMeshProUGUI playerCrit;

    TextMeshProUGUI targetName;
    TextMeshProUGUI targetHP;
    TextMeshProUGUI targetDamage;
    TextMeshProUGUI targetHit;
    TextMeshProUGUI targetCrit;

    // Start is called before the first frame update
    void Start()
    {
        previewPanel = transform.Find("PreviewPanel").gameObject;

        playerName = previewPanel.transform.Find("PlayerName").GetComponent<TextMeshProUGUI>();
        playerHP = previewPanel.transform.Find("PlayerHP").GetComponent<TextMeshProUGUI>();
        playerDamage = previewPanel.transform.Find("PlayerDamage").GetComponent<TextMeshProUGUI>();
        playerHit = previewPanel.transform.Find("PlayerHit").GetComponent<TextMeshProUGUI>();
        playerCrit = previewPanel.transform.Find("PlayerCrit").GetComponent<TextMeshProUGUI>();

        targetName = previewPanel.transform.Find("TargetName").GetComponent<TextMeshProUGUI>();
        targetHP = previewPanel.transform.Find("TargetHP").GetComponent<TextMeshProUGUI>();
        targetDamage = previewPanel.transform.Find("TargetDamage").GetComponent<TextMeshProUGUI>();
        targetHit = previewPanel.transform.Find("TargetHit").GetComponent<TextMeshProUGUI>();
        targetCrit = previewPanel.transform.Find("TargetCrit").GetComponent<TextMeshProUGUI>();

        previewPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPanel(CharacterInfo player, CharacterInfo target, WeaponData selectedWeapon, WeaponData targetWeapon, int playerAttack, int targetAttack)
    {
        playerName.text = player.characterData.name;
        playerHP.text = player.characterData.HP_CURRENT.ToString();
        playerDamage.text = playerAttack.ToString();
        playerHit.text = ((player.characterData.AIM + selectedWeapon.HIT) - (target.characterData.DODGE + target.currentTile.dodgeBoost)).ToString();
        playerCrit.text = (player.characterData.CRIT + selectedWeapon.CRIT).ToString();

        targetName.text = target.characterData.name;
        targetHP.text = target.characterData.HP_CURRENT.ToString();
        if (targetWeapon != null && player.EvaluateIsEnemy(target))
        {
            targetDamage.text = targetAttack.ToString();
            targetHit.text = ((target.characterData.AIM + targetWeapon.HIT) - (player.characterData.DODGE + player.currentTile.dodgeBoost)).ToString();
            targetCrit.text = (target.characterData.CRIT + selectedWeapon.CRIT).ToString();
        }
        else
        {
            targetDamage.text = "--";
            targetHit.text = "--";
            targetCrit.text = "--";
        }

        previewPanel.SetActive(true);
    }

    public void DisablePanel()
    {
        playerName.text = "";
        playerHP.text = "";
        playerDamage.text = "";
        playerHit.text = "";
        playerCrit.text = "";

        targetName.text = "";
        targetHP.text = "";
        targetDamage.text = "";
        targetHit.text = "";
        targetCrit.text = "";

        previewPanel.SetActive(false);
    }
}
