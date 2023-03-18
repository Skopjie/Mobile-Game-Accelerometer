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

    public void UpdateLobbyList(Lobby lobby)
    {
        Debug.Log("Players in Lobby " + lobby.Name + " " + lobby.Data["GameMode"].Value);

        SetLobbyData(lobby);

        foreach (Transform child in playerListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Player player in lobby.Players)
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
}
