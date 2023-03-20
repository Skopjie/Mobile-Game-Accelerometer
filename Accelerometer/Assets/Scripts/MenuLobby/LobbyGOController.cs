using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Lobbies.Models;

public class LobbyGOController : MonoBehaviour
{
    [SerializeField] string idLobby;

    [Header("Textos")]
    [SerializeField] TextMeshProUGUI nameLobbyText;
    [SerializeField] TextMeshProUGUI numPlayerLobbyText;

    public void ShowUpdateLobbyData(Lobby lobby) {
        gameObject.SetActive(true);
        nameLobbyText.text = lobby.Name;
        numPlayerLobbyText.text = lobby.Players.Count + "/" + lobby.MaxPlayers;

        idLobby = lobby.Id;
    }

    public void HideLobby() {
        idLobby = "";
        gameObject.SetActive(false);
    }

    public void JoinLobby() {
        Debug.Log("Conectandose al lobby: " + idLobby);
        LobbyController.Instance.JoinLobbyByCode(idLobby);
    }
}
