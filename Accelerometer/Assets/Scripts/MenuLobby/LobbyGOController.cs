using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Lobbies.Models;

public class LobbyGOController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameLobbyText;
    [SerializeField] TextMeshProUGUI numPlayerLobbyText;

    public void UpdateLobbyData(Lobby lobby)
    {
        nameLobbyText.text = lobby.Name;
        numPlayerLobbyText.text = lobby.Players.Count +"/"+lobby.MaxPlayers;
    }
}
