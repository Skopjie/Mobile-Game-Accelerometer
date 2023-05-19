using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MultiplayerUI: MonoBehaviour
{
    [Header("Botones")]
    [SerializeField] Button createLobbyButton;
    [SerializeField] Button joinPublicButton;
    [SerializeField] Button joinPrivateButton;
    [SerializeField] Button backButton;

    [Header("Componentes")]
    [SerializeField] CreateLobbyUI createLobbyUI;
    [SerializeField] LobbyListUI lobbyListUI;

    private void Awake() {
        createLobbyButton.onClick.AddListener(() => {
            LobbyUIController.Instance.ShowCreateLobby();
        });

        joinPublicButton.onClick.AddListener(() => {
            LobbyUIController.Instance.ShowLobbyList();
        });

        joinPrivateButton.onClick.AddListener(() => {
            HideCanvas();
        });

        backButton.onClick.AddListener(() => {
            LobbyUIController.Instance.ShowMainMenu();
        });
    }

    public void HideCanvas() {
        gameObject.SetActive(false);
    }

    public void ShowCanvas() {
        gameObject.SetActive(true);
    }

}
