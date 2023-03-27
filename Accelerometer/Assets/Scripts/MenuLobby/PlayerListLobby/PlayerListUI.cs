using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using TMPro;
using UnityEngine;
using Unity.Services.Authentication;
using UnityEngine.UI;

public class PlayerListUI : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] Transform playerListContainer;
    [SerializeField] GameObject playerPanelPrefab;
    [SerializeField] GameObject startGameButton;

    [Header("Textos")]
    [SerializeField] TextMeshProUGUI nameLobbyText;
    [SerializeField] TextMeshProUGUI numPlayersLobbyText;

    List<PlayerSingleGOController> playerLobbyList = new List<PlayerSingleGOController>();

    private void Start() {
        InitPlayersLobby();

        LobbyController.Instance.OnJoinedLobby += UpdatePlayerList;
        LobbyController.Instance.OnJoinedLobbyUpdate += UpdatePlayerList;
        LobbyController.Instance.OnKickedFromLobby += HideCanvasEvent;
    }

    public void SetLobbyData(Lobby lobby)
    {
        nameLobbyText.text = lobby.Name;
        numPlayersLobbyText.text = lobby.Players.Count+"/"+lobby.MaxPlayers;
    }

    void InitPlayersLobby() {
        for (int i = 0; i < 4; i++) {
            GameObject playerLobbySingleGO = Instantiate(playerPanelPrefab, playerListContainer);
            PlayerSingleGOController playerSingleController = playerLobbySingleGO.GetComponent<PlayerSingleGOController>();
            playerLobbyList.Add(playerSingleController);
            playerSingleController.HidePlayerGO();
        }
    }

    public void UpdatePlayerList(object sender, LobbyController.LobbyEventArgs e) {
        HideAllPlayerLobbyList();
        SetLobbyData(e.lobby);

        if (LobbyController.Instance.IsLobbyHost()) {
            ShowButtonPlay();
        }

        Debug.Log("Players in Lobby " + e.lobby.Name + " " + e.lobby.Data["GameMode"].Value);
        for (int i = 0; i < e.lobby.Players.Count; i++) {
            bool isKichable = LobbyController.Instance.IsLobbyHost() &&
                e.lobby.Players[i].Id != AuthenticationService.Instance.PlayerId;

            playerLobbyList[i].UpdateLobbyData(e.lobby.Players[i], isKichable);
        }
    }

    public void HideAllPlayerLobbyList() {
        foreach (PlayerSingleGOController playerLobbySingleController in playerLobbyList) {
            playerLobbySingleController.HidePlayerGO();
        }
    }

    void ShowButtonPlay() {
        startGameButton.SetActive(true);
    }


    public void ExitLobby()
    {
        LobbyController.Instance.LeaveLobby();
    }

    public void ShowCanvas() {
        gameObject.SetActive(true);
    }

    public void HideCanvas() {
        gameObject.SetActive(false);
        startGameButton.SetActive(false);
    }

    public void HideCanvasEvent(object sender, LobbyController.LobbyEventArgs e) {
        gameObject.SetActive(false);
        startGameButton.SetActive(false);
        LobbyListUI.Instance.ShowCanvas();
    }
}
