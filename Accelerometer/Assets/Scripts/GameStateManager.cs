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

    public bool gameIsStarted = false;

    [Header("Variable")]
    [SerializeField] int timmerStartRound;
    [SerializeField] float actualTimmerStartRound;
    [SerializeField] GameCanvasUI canvasGame;



    public event EventHandler<GameStateEventArgs> OnRoundIsOver;
    public event EventHandler<GameStateEventArgs> OnAllPlayerConnected;
    public event EventHandler<GameStateEventArgs> OnRoundStart;

    public class GameStateEventArgs : EventArgs {
        public string lobby;
    }

    private void Start() {
        instace = this;

        LobbyUIController.Instance.OnStartCinematicEnds += cronometro;
    }

    private void Update() {
        cronometroefe();
    }

    public void SetGameData(int newNumberPlater, int newNumberRound) {
        numberPlayer.Value = newNumberPlater;
        actualNumberOfPlayers.Value = newNumberPlater;

        numberRounds.Value = newNumberRound;
        actualNumberRounds.Value = newNumberRound;
    }

    void cronometro(object sender, LobbyUIController.LobbyUIHandler e) {
        canvasGame.ShowTimmer();
        gameIsStarted = true;
    }

    void cronometroefe() {
        if (gameIsStarted) {
            actualTimmerStartRound -= Time.deltaTime;
            canvasGame.SetTimmerText((int)actualTimmerStartRound);
            if (actualTimmerStartRound <= 0) {
                canvasGame.HideTimmer();
                actualTimmerStartRound = timmerStartRound;
                StartGame();
                OnRoundStart?.Invoke(this, new GameStateEventArgs { lobby = "" });
            }
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
        }
    }

    public void PlayerEliminated() {
        actualNumberOfPlayers.Value--;
    }

    [ClientRpc]
    public void CheckNumberOfPlayersClientRpc() {
        if (actualNumberOfPlayers.Value == 1) {
            OnRoundIsOver?.Invoke(this, new GameStateEventArgs { lobby = "" });

            if(IsHost)
                actualNumberOfPlayers.Value = numberPlayer.Value;

            Debug.Log("Fin");
            GameIsOver();
        }
    }

    public void StartGame() {
        gameIsStarted = false;
        if (IsHost)
            TableMap.Instance.StartRoundGameInvoke();
    }

    public void GameIsOver() {
        if (IsHost)
            actualNumberRounds.Value--;

        if(actualNumberRounds.Value == 0) {
            Debug.Log("Fin del juego llevar a Lobby");
        }

        TableMap.Instance.StopGame();
        //Poner panel de game over
        //sistema de rondas y numero de victorias
        //Activar jugadores y ponerlos en el mismo lugar 
        //reiniciar mapaW
        //si rondas acabadas mostrar podium
        //volver al lobby
    }
}
