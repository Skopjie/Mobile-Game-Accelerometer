using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameStateManager : NetworkBehaviour {

    public static GameStateManager Instance { get { return instace; } }
    private static GameStateManager instace;

    public NetworkVariable<int> numberPlayer =
    new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    public NetworkVariable<int> actualNumberOfPlayers =
    new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    public NetworkVariable<int> numberRounds =
    new NetworkVariable<int>(2,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    public NetworkVariable<int> actualNumberRounds =
    new NetworkVariable<int>(2,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);


    [Header("Variable")]
    public List<NetworkObject> playersNetwork = new List<NetworkObject>();



    public event EventHandler<GameStateEventArgs> OnRoundIsOver;
    public event EventHandler<GameStateEventArgs> OnAllPlayerConnected;
    public event EventHandler<GameStateEventArgs> OnRoundStart;

    public class GameStateEventArgs : EventArgs {
        public string lobby;
    }

    private void Start() {
        instace = this;
    }

    void UnsubscribeAllEvnts() {
        if (OnRoundIsOver != null)
            foreach (var d in OnRoundIsOver.GetInvocationList())
                OnRoundIsOver -= (d as EventHandler<GameStateEventArgs>);

        if (OnAllPlayerConnected != null)
            foreach (var d in OnAllPlayerConnected.GetInvocationList())
                OnAllPlayerConnected -= (d as EventHandler<GameStateEventArgs>);
        OnAllPlayerConnected += LobbyUIController.Instance.ActiveStartGameAnimation;

        if (OnRoundStart != null)
            foreach (var d in OnRoundStart.GetInvocationList())
                OnRoundStart -= (d as EventHandler<GameStateEventArgs>);
    }

    public void SetGameData(int newNumberPlater, int newNumberRound) {
        if(newNumberPlater != 0) {
            numberPlayer.Value = newNumberPlater;
            actualNumberOfPlayers.Value = newNumberPlater;
        }
        if(newNumberRound != 0) {
            numberRounds.Value = newNumberRound;
            actualNumberRounds.Value = newNumberRound;
        }
    }

    [ClientRpc]
    public void AddNewPlayerClientRpc() {
        if (IsHost) {
            numberPlayer.Value++;
            actualNumberOfPlayers.Value++;
        }
        Invoke("CheckAllPlayerAreConnected", 1);
    }

    void CheckAllPlayerAreConnected() {
        if (numberPlayer.Value == RelayController.Instance.relayData.numberPlayer) {
            OnAllPlayerConnected?.Invoke(this, new GameStateEventArgs { lobby = "" });
            SetGameData(0, 2);
        }
    }

    public void PlayerEliminated() {
        actualNumberOfPlayers.Value--;
    }

    [ClientRpc]
    public void CheckNumberOfPlayersClientRpc() {
        if (actualNumberOfPlayers.Value == 1 || actualNumberOfPlayers.Value == 0) {
            if (IsHost)
                actualNumberOfPlayers.Value = numberPlayer.Value;
            GameIsOver();
        }
    }

    public void StartGame() {
        OnRoundStart?.Invoke(this, new GameStateEventArgs { lobby = "" });
        if (IsHost)
            TableMap.Instance.StartRoundGameInvoke();
    }

    void InvokeNextRound() {
        OnRoundIsOver?.Invoke(this, new GameStateEventArgs { lobby = "" });
        TableMap.Instance.StopGame();
        LobbyUIController.Instance.StartChronometerAnimation();
    }

    public void GameIsOver() {
        if (IsHost)
            actualNumberRounds.Value--;

        if (actualNumberRounds.Value > 0) {
            InvokeNextRound();
        }
        else if (actualNumberRounds.Value <= 0) {
            LobbyUIController.Instance.ShowMultiplayerOptions();
            ResetDataGame();
            ShutDownNetworkObjects();
        }
    }

    public void ResetDataGame() {
        numberPlayer.Value = 0;
        actualNumberOfPlayers.Value = 0;

        numberRounds.Value = 0;
        actualNumberRounds.Value = 0;
    }

    public void ShutDownNetworkObjects() {
        NetworkManager.Singleton.Shutdown();

        playersNetwork.Clear();
        TableMap.Instance.DeleteAllDataTable();

        UnsubscribeAllEvnts();
    }
}
