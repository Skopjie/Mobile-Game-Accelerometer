using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameCanvasUI : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] TextMeshProUGUI timmerText;
    [SerializeField] TextMeshProUGUI playerGameOverText;

    [Header("Variable")]
    [SerializeField] int timmerStartRound;
    [SerializeField] float actualTimmerStartRound;

    public void SetTimmerText(int newTimme) {
        timmerText.text = "" + newTimme;
    }

    public void ShowTimmer() {
        timmerText.gameObject.SetActive(true);
    }

    public void HideTimmer() {
        timmerText.gameObject.SetActive(false);
    }

    public void ShowCanvas() {
        gameObject.SetActive(true);
    }

    public void HideCanvas() {
        gameObject.SetActive(false);
    }
}
