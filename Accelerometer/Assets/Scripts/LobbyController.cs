using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.Events;
using Unity.Services.Lobbies.Models;
using UnityEngine.SceneManagement;
using System;

public struct LobbyInfo
{
    public bool IsPrivate;
    public string nameLobby;
    public int numPlayer;
}

public class LobbyController : MonoBehaviour
{
    public static LobbyController Instance { get { return instace; } }
    private static LobbyController instace;

    public const string KEY_PLAYER_NAME = "PlayerName";
    public const string KEY_PLAYER_CHARACTER = "Character";
    public const string KEY_GAME_MODE = "GameMode";
    public const string KEY_RELAY = "RelayCode";


    private Lobby joinedLobby;
    private string playerName;

    [SerializeField] private float heartbeatTimer;
    [SerializeField] private float lobbyPollTimer;


    public event EventHandler OnLeftLobby;

    public event EventHandler<LobbyEventArgs> OnJoinedLobby;
    public event EventHandler<LobbyEventArgs> OnJoinedLobbyUpdate;
    public event EventHandler<LobbyEventArgs> OnKickedFromLobby;
    public event EventHandler<LobbyEventArgs> OnLobbyGameModeChanged;
    public event EventHandler<LobbyEventArgs> OnGameStart;
    public class LobbyEventArgs : EventArgs
    {
        public Lobby lobby;
    }

    public event EventHandler<OnLobbyListChangedEventArgs> OnLobbyListChanged;
    public class OnLobbyListChangedEventArgs : EventArgs
    {
        public List<Lobby> lobbyList;
    }


    private void Awake()
    {
        instace = this;
    }

    void Start()
    {
        InvokeRepeating("HandleLobbyHeartbeat", heartbeatTimer, heartbeatTimer);
        InvokeRepeating("HandleLobbyPoolUpdates", lobbyPollTimer, lobbyPollTimer);
    }

    #region SetterInfoLobby
    public void SetPlayerName(string newPlayerName)
    {
        playerName = newPlayerName;
    }
    #endregion

    public async void AuthenticatioMultiplayer(string playerName)
    {
        this.playerName = playerName;
        InitializationOptions initializationOptions = new InitializationOptions();
        initializationOptions.SetProfile(playerName);

        await UnityServices.InitializeAsync(initializationOptions);


        AuthenticationService.Instance.SignedIn += () => {
            // do nothing
            Debug.Log("Signed in! " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        Debug.Log("Inicio online: " + playerName);
    }

    async void HandleLobbyHeartbeat()
    {
        if(IsLobbyHost())
        {
            await LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
        }
    }

    async void HandleLobbyPoolUpdates()
    {
        if (joinedLobby != null)
        {
            joinedLobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);

            //print players
            OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });

            if (!IsPlayerInLobby())
            {
                // Player was kicked out of this lobby
                Debug.Log("Kicked from Lobby!");

                OnKickedFromLobby?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });

                joinedLobby = null;
            }

            if(joinedLobby.Data[KEY_RELAY].Value != "0")
            {
                //el lobby ha comenzado a jugar
                if(!IsLobbyHost()) //host se unio al relay asique no tener en cuenta a este 
                {
                    LobbyUIController.Instance.ShowLoadingMenu();
                    RelayController.Instance.SetRelayData(joinedLobby.Data[KEY_RELAY].Value, playerName, joinedLobby.Players.Count, false);
                    RelayController.Instance.JoinRelay();
                    joinedLobby = null;
                }
                //Invocaar comienzo juego
            }
        }
    }

    public async void CreateLobby(LobbyInfo lobbyInfo)
    {
        try
        {
            Debug.Log(lobbyInfo.nameLobby + " / " + lobbyInfo.numPlayer + " / " + lobbyInfo.IsPrivate);
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = lobbyInfo.IsPrivate,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>
                {
                    { KEY_GAME_MODE, new DataObject(DataObject.VisibilityOptions.Public, "Eassy", DataObject.IndexOptions.S1) },
                    { KEY_RELAY, new DataObject(DataObject.VisibilityOptions.Member, "0") }
                }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyInfo.nameLobby, lobbyInfo.numPlayer, createLobbyOptions);

            joinedLobby = lobby;

            //PrintPlayers(hostLobby);
            OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });

            Debug.Log("Se creo una patida llamada: " + lobby.Name);
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
            QueryLobbiesOptions options = new QueryLobbiesOptions();
            options.Count = 25;

            // Filter for open lobbies only
            options.Filters = new List<QueryFilter>()
            {
                new QueryFilter(
                    field: QueryFilter.FieldOptions.AvailableSlots,
                    op: QueryFilter.OpOptions.GT,
                    value: "0")
            };

            QueryResponse query = await Lobbies.Instance.QueryLobbiesAsync(options);
            Debug.Log("Lobbies found: " +query.Results.Count);

            OnLobbyListChanged?.Invoke(this, new OnLobbyListChangedEventArgs { lobbyList = query.Results });
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

            //es anonimo porque me no funciona con parrelSync, mismo usurio
            Lobby lobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobbyCode, joinLobbyCodeOptions);
            joinedLobby = lobby;

            if(joinedLobby != null)
                OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    /*async void UpdateLobbyOption(string gameMode)
    {
        if(joinedLobby != null)
        {
            try
            {
                joinedLobby = await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject>
                {
                    {KEY_GAME_MODE, new DataObject(DataObject.VisibilityOptions.Public, gameMode) },
                    { KEY_RELAY, new DataObject(DataObject.VisibilityOptions.Member, "0") }
                }
                });

            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }*/

    public async void LeaveLobby()
    {
        if(joinedLobby != null)
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);

                joinedLobby = null;

                OnLeftLobby?.Invoke(this, EventArgs.Empty);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }  
        }
    }

    public async void KickPlayers(string playerId)
    {
        if (IsLobbyHost())
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    public async void StartGame()
    {
        if(IsLobbyHost())
        {
            try
            {
                string relayCode = await RelayController.Instance.CreateRelay(playerName, joinedLobby.Players.Count);

                Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject>
                    {
                        {KEY_RELAY, new DataObject(DataObject.VisibilityOptions.Member, relayCode) }
                    }
                });
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    public Lobby GetJoinedLobby()
    {
        return joinedLobby;
    }

    public bool IsLobbyHost()
    {
        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    private bool IsPlayerInLobby()
    {
        if (joinedLobby != null && joinedLobby.Players != null)
        {
            foreach (Player player in joinedLobby.Players)
            {
                if (player.Id == AuthenticationService.Instance.PlayerId)
                {
                    // This player is in this lobby
                    return true;
                }
            }
        }
        return false;
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

    public async void DeleteLobby() {
        try {
            if(joinedLobby != null) {
                await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
                joinedLobby = null;
            }
        } catch (LobbyServiceException e) {
            Debug.Log(e);
        }

    }

    private Player GetPlayer()
    {
        return new Player(AuthenticationService.Instance.PlayerId, null, new Dictionary<string, PlayerDataObject> {
            { KEY_PLAYER_NAME, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerName) }
        });
    }
}
