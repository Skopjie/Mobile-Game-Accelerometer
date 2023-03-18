using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.Events;
using Unity.Services.Lobbies.Models;

public struct LobbyInfo
{
    public bool IsPrivate;
    public string lobbyName;
    public int numPlayer;
}

public class LobbyController : MonoBehaviour
{

    public static LobbyController Instance { get { return instace; } }
    private static LobbyController instace;

    private Lobby hostLobby;
    private Lobby joinedLobby;


    private string playerName;

    public LobbyInfo lobbyInfo;

    public UnityEvent<List<Lobby>> OnRefreshLobbyList;
    public UnityEvent<Lobby> OnRefreshPlayerList;
    public UnityEvent OnJoinLobby;


    private void Awake()
    {
        instace = this;
    }

    void Start()
    {
        InvokeRepeating("HandleLobbyHeartbeat", 10, 10);
        InvokeRepeating("HandleLobbyPoolUpdates", 1.1f, 1.1f);
    }

    public void SetPlayerName(string newPlayerName)
    {
        playerName = newPlayerName;
    }

    public void SetNameLobby(string newLobbyName)
    {
        lobbyInfo.lobbyName = newLobbyName;
    }

    public void SetNumerPlayerLobby(int newNumPlayer)
    {
        lobbyInfo.numPlayer = newNumPlayer;
    }

    public void SetProtectionLobby(bool newProtectionType)
    {
        lobbyInfo.IsPrivate = newProtectionType;
    }



    public async void AuthenticatioMultiplayer()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        Debug.Log("Inicio online: " + playerName);
    }

    async void HandleLobbyHeartbeat()
    {
        if(hostLobby != null)
        {
            await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
        }
    }

    async void HandleLobbyPoolUpdates()
    {
        if (joinedLobby != null)
        {
            Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
            PrintPlayers(lobby);
            joinedLobby = lobby;
        }
    }

    public async void CreateLobby()
    {
        try
        {
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = lobbyInfo.IsPrivate,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>
                {
                    { "GameMode", new DataObject(DataObject.VisibilityOptions.Public, "Eassy", DataObject.IndexOptions.S1) }
                }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyInfo.lobbyName, lobbyInfo.numPlayer, createLobbyOptions);

            Debug.Log("Se creo una patida llamada: " + lobbyInfo.lobbyName +" de" + lobby.MaxPlayers +
                " jugadores, es una partida privada = " + lobbyInfo.IsPrivate +" su id es: " + lobby.Id);

            hostLobby = lobby;
            joinedLobby = hostLobby;

            PrintPlayers(hostLobby);
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void ListLobbies()
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
            Debug.Log("Lobbies found: " +query.Results.Count);

            OnRefreshLobbyList?.Invoke(query.Results);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            JoinLobbyByIdOptions joinLobbyCodeOptions = new JoinLobbyByIdOptions
            {
                Player = GetPlayer()
            };
            Debug.Log("Vuamos");
            //es anonimo porque me no funciona con parrelSync, mismo usurio
            Lobby lobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobbyCode, joinLobbyCodeOptions);
            joinedLobby = lobby;
            Debug.Log("Vuamos2");
            PrintPlayers(lobby);

            OnJoinLobby?.Invoke();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private void PrintPlayers(Lobby lobby)
    {
        OnRefreshPlayerList?.Invoke(lobby);
    }

    async void UpdateLobbyOption(string gameMode)
    {
        try
        {
            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    {"GameMode", new DataObject(DataObject.VisibilityOptions.Public, gameMode) }
                }
            });

            joinedLobby = hostLobby;
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    async void KickPlayers(int idPlayer)
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, joinedLobby.Players[idPlayer].Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    /*async void MigrateLobbyHost()
    {
        try
        {
            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions
            {
                HostId = joinedLobby.Players[1].Id
            });

            joinedLobby = hostLobby;
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }*/

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
