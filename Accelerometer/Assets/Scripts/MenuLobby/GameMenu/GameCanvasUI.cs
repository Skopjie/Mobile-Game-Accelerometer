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

    bool gameIsStarted = false;

    private void Awake() {
        LobbyUIController.Instance.OnStartCinematicEnds += StartChronometer;
    }

    private void FixedUpdate() {
        UpdateChronometer();
    }

    void StartChronometer(object sender, LobbyUIController.LobbyUIHandler e) {
        actualTimmerStartRound = timmerStartRound;
        ShowTimmer();
        gameIsStarted = true;
    }

    void UpdateChronometer() {
        if (gameIsStarted) {
            actualTimmerStartRound -= Time.deltaTime;
            SetTimmerText((int)actualTimmerStartRound);
            if (actualTimmerStartRound <= 0) {
                HideTimmer();
                actualTimmerStartRound = timmerStartRound;
                gameIsStarted = false;
                if (GameStateManager.Instance.isInMultiplayer) {
                    GameStateManager.Instance.StartGame();
                }
                else {
                    GameStateManager.Instance.StartGameSinglePlayer();
                }
            }
        }
    }

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
