using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Unity.Services.Lobbies.Models;

public class LobbyGOController : MonoBehaviour
{
    [Header("Datos Importantes")]
    [SerializeField] string idLobby;

    [Header("Componentes")]
    [SerializeField] LobbyListUI lobbyListUI;

    [Header("Botones")]
    [SerializeField] Button joinLobbyButton;

    [Header("Textos")]
    [SerializeField] TextMeshProUGUI nameLobbyText;
    [SerializeField] TextMeshProUGUI numPlayerLobbyText;

    private void Awake() {
        joinLobbyButton.onClick.AddListener(() => {
            JoinLobby();
        });
    }

    public void InitData(LobbyListUI lobbyList) {
        lobbyListUI = lobbyList;
    }

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
        LobbyUIController.Instance.ShowPlayerList();
    }
}
