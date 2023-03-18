using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyListUI : MonoBehaviour
{
    [SerializeField] Transform lobbyListContainer;
    [SerializeField] GameObject lobbyPanelPrefab;

    public void UpdateLobbyList(List<Lobby> lobbyList)
    {
        foreach (Transform child in lobbyListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Lobby lobby in lobbyList)
        {
            LobbyGOController lobbySingle = Instantiate(lobbyPanelPrefab, lobbyListContainer).GetComponent<LobbyGOController>();
            lobbySingle.UpdateLobbyData(lobby);
            Debug.Log(lobby.Name + " / " + lobby.MaxPlayers + " / " + lobby.Data["GameMode"].Value);
        }
    }
}
