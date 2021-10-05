using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsButtonController : MonoBehaviour
{
    [SerializeField] private GameObject creditsPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void DisableCredits()
    {
        creditsPanel.SetActive(false);
    }
}
