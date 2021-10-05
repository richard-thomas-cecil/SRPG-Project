using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultsScreenController : MonoBehaviour
{
    private TextMeshProUGUI resultsText;

    // Start is called before the first frame update
    void Start()
    {
        resultsText = transform.Find("ResultsText").gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableScreen()
    {
        gameObject.SetActive(false);
    }

    public void DisplayResult(RESULT_TYPE result)
    {
        if(result == RESULT_TYPE.VICTORY)
        {
            resultsText.text = "Victory";
        }
        else if(result == RESULT_TYPE.DEFEAT)
        {
            resultsText.text = "Defeat";
        }

        gameObject.SetActive(true);
    }
}
