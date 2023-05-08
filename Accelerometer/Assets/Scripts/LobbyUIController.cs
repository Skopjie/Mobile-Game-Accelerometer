using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LobbyState {

}

public class LobbyUIController : MonoBehaviour
{
    public static LobbyUIController Instance { get { return instace; } }
    private static LobbyUIController instace;

    public LobbyState lobbyState;

    [Header("Componentes")]
    [SerializeField] MainMenuUI mainMenuUI;
    [SerializeField] ConnectMultiplayerUI connectMultiplayerUI;
    [SerializeField] MultiplayerUI multiplayerUI;
    [SerializeField] CreateLobbyUI createLobbyUI;
    [SerializeField] LobbyListUI lobbyListUI;
    [SerializeField] PlayerListUI playerListUI;
    [SerializeField] MenuLoadingUI menuLoadingUI;
    [SerializeField] GameCanvasUI gameCanvasUI;

    [Header("Camaras")]
    [SerializeField] CinemachineVirtualCamera connectMultiplayerCamera;
    [SerializeField] CinemachineVirtualCamera createLobbyCamera;
    [SerializeField] CinemachineVirtualCamera lobbyListCamera;
    [SerializeField] CinemachineVirtualCamera playerListCamera;
    [SerializeField] CinemachineVirtualCamera gameCamera;


    public event EventHandler<LobbyUIHandler> OnStartCinematicEnds;

    public class LobbyUIHandler : EventArgs {
        public string lobby;
    }

    private void Awake() {
        instace = this;
    }

    private void Start() {
        GameStateManager.Instance.OnAllPlayerConnected += StartGameCamera;
    }

    public void HideAllCanvas() {
        mainMenuUI.HideCanvas();
        connectMultiplayerUI.HideCanvas();
        multiplayerUI.HideCanvas();
        createLobbyUI.HideCanvas();
        lobbyListUI.HideCanvas();
        playerListUI.HideCanvas();
        menuLoadingUI.HideCanvas();
        gameCanvasUI.HideCanvas();
    }

    public void DisableCameras() {
        connectMultiplayerCamera.Priority = 0;
        createLobbyCamera.Priority = 0;
        lobbyListCamera.Priority = 0;
        playerListCamera.Priority = 0;
    }

    public void ShowMultiplayerOptions() {
        //Oculta anterior canvas
        connectMultiplayerUI.HideCanvas();
        createLobbyUI.HideCanvas();
        lobbyListUI.HideCanvas();
        //mueve camara o transicion
        DisableCameras();
        connectMultiplayerCamera.Priority = 10;
        //esperar a transicion
        //muestra nuevo canvas y hace la inicializacion correspondiente
        multiplayerUI.ShowCanvas();
    }

    public void ShowLobbyList() {
        //Oculta anterior canvas
        playerListUI.HideCanvas();
        multiplayerUI.HideCanvas();
        //mueve camara o transicion
        DisableCameras();
        lobbyListCamera.Priority = 10;
        //esperar a transicion
        //muestra nuevo canvas y hace la inicializacion correspondiente
        lobbyListUI.ShowCanvas();
    }

    public void ShowCreateLobby() {
        //Oculta anterior canvas
        multiplayerUI.HideCanvas();
        //mueve camara o transicion
        DisableCameras();
        createLobbyCamera.Priority = 10;
        //esperar a transicion
        //muestra nuevo canvas y hace la inicializacion correspondiente
        createLobbyUI.ShowCanvas();
    }

    public void ShowPlayerList() {
        //Oculta anterior canvas
        createLobbyUI.HideCanvas();
        lobbyListUI.HideCanvas();
        //mueve camara o transicion
        DisableCameras();
        playerListCamera.Priority = 10;
        //esperar a transicion
        //muestra nuevo canvas y hace la inicializacion correspondiente
        playerListUI.ShowCanvas();
    }

    public void ShowLoadingMenu() {
        playerListUI.HideCanvas();
        menuLoadingUI.ShowCanvas();
    }

    public void ShowChronometer() {
        HideAllCanvas();
        gameCanvasUI.ShowCanvas();
    }

    public void StartGameCamera(object sender, GameStateManager.GameStateEventArgs e) {
        StartCoroutine(StartAnimation());
    }
    public void StartGameCamera() {
        StartCoroutine(StartAnimation());
    }

    IEnumerator StartAnimation() {
        HideAllCanvas();
        yield return new WaitForSeconds(1);
        DisableCameras();
        gameCamera.Priority = 10;
        yield return new WaitForSeconds(1);
        //tengo que activarlo para que se invoque evento
        ShowChronometer();
        OnStartCinematicEnds?.Invoke(this, new LobbyUIHandler { lobby = "" });
    }
}
