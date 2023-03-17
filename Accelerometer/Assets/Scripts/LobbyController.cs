using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Authentication;
using UnityEngine;
using Unity.Services.Lobbies.Models;

public class LobbyController : MonoBehaviour
{

    private Lobby hostLobby;
    private string playerName;

    // Start is called before the first frame update
    async void Start()
    {
        await UnityServices.InitializeAsync();

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        playerName = "Code " + UnityEngine.Random.Range(0, 99);

        InvokeRepeating("HandleLobbyHeartbeat", 10, 10);
    }

    async void HandleLobbyHeartbeat()
    {
        if(hostLobby != null)
        {
            await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
        }
    }

    private async void CreateLobby()
    {
        try
        {
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = GetPlayer()
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync("peter", 4, createLobbyOptions);
            hostLobby = lobby; 
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryOptions = new QueryLobbiesOptions
            {
                Count = 25,
                Filters = new List<QueryFilter>
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0" , QueryFilter.OpOptions.GT)
                }
            };

            QueryResponse query = await Lobbies.Instance.QueryLobbiesAsync();

            foreach (Lobby lobby in query.Results)
                Debug.Log(lobby.Name + " / " + lobby.MaxPlayers);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            JoinLobbyByCodeOptions joinLobbyCodeOptions = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer()
            };

            await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyCodeOptions);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private void PrintPlayers(Lobby lobby)
    {
        foreach(Player player in lobby.Players)
        {

        }
    }

    private Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
                    {
                        {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member ,playerName) }
                    }
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
