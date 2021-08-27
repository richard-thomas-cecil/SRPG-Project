using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponInfoController : MonoBehaviour
{
    public GameObject weaponInfoPanel;

    private TextMeshProUGUI weaponName;
    private TextMeshProUGUI weaponAttack;
    private TextMeshProUGUI weaponHit;
    private TextMeshProUGUI weaponCrit;

    // Start is called before the first frame update
    void Start()
    {
        weaponInfoPanel = transform.Find("WeaponInfoPanel").gameObject;

        weaponName = weaponInfoPanel.transform.Find("WeaponName").GetComponent<TextMeshProUGUI>();
        weaponAttack = weaponInfoPanel.transform.Find("WeaponDamage").GetComponent<TextMeshProUGUI>();
        weaponHit = weaponInfoPanel.transform.Find("WeaponHit").GetComponent<TextMeshProUGUI>();
        weaponCrit = weaponInfoPanel.transform.Find("WeaponCrit").GetComponent<TextMeshProUGUI>();

        weaponInfoPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPanel(WeaponData weapon, GameObject weaponButtonObject)
    {
        weaponInfoPanel.GetComponent<RectTransform>().position = new Vector3((weaponButtonObject.transform.position.x - weaponButtonObject.GetComponent<RectTransform>().rect.width - 50), weaponButtonObject.transform.position.y, weaponButtonObject.transform.position.z);

        weaponName.text = weapon.weaponName;
        weaponAttack.text = "ATK: " + weapon.ATK;
        weaponHit.text = "HIT: " + weapon.HIT;
        weaponCrit.text = "CRIT: " + weapon.CRIT;

        weaponInfoPanel.SetActive(true);
    }

    public void DisablePanel()
    {
        weaponInfoPanel.SetActive(false);
    }
}
