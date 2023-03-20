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

    private void Start() {
        LobbyController.Instance.OnJoinedLobby += UpdateLobbyList;
        LobbyController.Instance.OnJoinedLobbyUpdate += UpdateLobbyList;
    }

    public void UpdateLobbyList(object sender, LobbyController.LobbyEventArgs e) {
        Debug.Log("Players in Lobby " + e.lobby.Name + " " + e.lobby.Data["GameMode"].Value);

        SetLobbyData(e.lobby);

        foreach (Transform child in playerListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Player player in e.lobby.Players)
        {
            PlayerSingleGOController playerSingle = Instantiate(playerPanelPrefab, playerListContainer).GetComponent<PlayerSingleGOController>();
            playerSingle.UpdateLobbyData(player);
            Debug.Log(player.Id + " / " + player.Data["PlayerName"].Value);
        }
    }

    public void SetLobbyData(Lobby lobby)
    {
        nameLobbyText.text = lobby.Name;
        numPlayersLobbyText.text = lobby.Players.Count+"/"+lobby.MaxPlayers;
    }

    public void ExitLobby()
    {
        LobbyController.Instance.LeaveLobby();
    }

    public void ShowCanvas() {
        gameObject.SetActive(true);
    }
}
