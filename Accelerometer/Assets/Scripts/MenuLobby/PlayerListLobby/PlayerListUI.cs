using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using TMPro;
using UnityEngine;

public class PlayerListUI : MonoBehaviour
{
    [SerializeField] Transform playerListContainer;
    [SerializeField] GameObject playerPanelPrefab;

    [Header("Textos")]
    [SerializeField] TextMeshProUGUI nameLobbyText;
    [SerializeField] TextMeshProUGUI numPlayersLobbyText;

    List<PlayerSingleGOController> playerLobbyList = new List<PlayerSingleGOController>();

    private void Start() {
        InitPlayersLobby();

        LobbyController.Instance.OnJoinedLobby += UpdatePlayerList;
        LobbyController.Instance.OnJoinedLobbyUpdate += UpdatePlayerList;
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
        Debug.Log("Players in Lobby " + e.lobby.Name + " " + e.lobby.Data["GameMode"].Value);
        for (int i = 0; i < e.lobby.Players.Count; i++) {
            playerLobbyList[i].UpdateLobbyData(e.lobby.Players[i]);
        }
    }

    public void HideAllPlayerLobbyList() {
        foreach (PlayerSingleGOController playerLobbySingleController in playerLobbyList) {
            playerLobbySingleController.HidePlayerGO();
        }
    }


    public void ExitLobby()
    {
        LobbyController.Instance.LeaveLobby();
    }

    public void ShowCanvas() {
        gameObject.SetActive(true);
    }
}
