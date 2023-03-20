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

    [Header("Componentes")]
    [SerializeField] CreateLobbyUI createLobbyUI;
    [SerializeField] LobbyListUI lobbyListUI;

    private void Awake() {
        createLobbyButton.onClick.AddListener(() => {
            HideCanvas();
            createLobbyUI.ShowCanvas();
        });

        joinPublicButton.onClick.AddListener(() => {
            HideCanvas();
            lobbyListUI.ShowCanvas();
        });

        joinPrivateButton.onClick.AddListener(() => {
            HideCanvas();
        });
    }

    public void HideCanvas() {
        gameObject.SetActive(false);
    }

    public void ShowCanvas() {
        gameObject.SetActive(true);
    }

}
