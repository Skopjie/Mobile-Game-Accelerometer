using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [Header("Botones")]
    [SerializeField] Button playOneButton;
    [SerializeField] Button multiplayerButton;


    private void Awake() {
        playOneButton.onClick.AddListener(() => {
            GameStateManager.Instance.isInMultiplayer = false;
            GameStateManager.Instance.InitMapSinglePlayer();
            LobbyUIController.Instance.ActiveStartGameAnimation();
        });
        multiplayerButton.onClick.AddListener(() => {
            GameStateManager.Instance.isInMultiplayer = true;
            LobbyUIController.Instance.ShowNameOnline();
        });
    }

    public void HideCanvas() {
        gameObject.SetActive(false);
    }
    public void ShowCanvas() {
        gameObject.SetActive(true);
    }
}
