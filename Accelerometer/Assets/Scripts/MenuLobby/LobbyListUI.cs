using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine.UI;
using UnityEngine;

public class LobbyListUI : MonoBehaviour
{
    public static LobbyListUI Instance { get { return instace; } }
    private static LobbyListUI instace;


    [Header("Componentes")]
    [SerializeField] Transform lobbyListContainer;
    [SerializeField] GameObject lobbyPanelPrefab;
    [SerializeField] PlayerListUI playerListUI;

    [Header("Botones")]
    [SerializeField] Button refreshLobbyListButton;
    [SerializeField] Button exitLobbyListButton;

    List<LobbyGOController> lobbyList = new List<LobbyGOController>();

    private void Awake() {
        refreshLobbyListButton.onClick.AddListener(() => {
            LobbyController.Instance.ListLobbies();
        });

        exitLobbyListButton.onClick.AddListener(() => {
            LobbyUIController.Instance.ShowMultiplayerOptions();
        });
    }

    private void Start() {
        instace = this;
        InitLobbyList();

        LobbyController.Instance.OnLobbyListChanged += UpdateLobbyList;
    }

    void InitLobbyList() {
        for (int i = 0; i < 25; i++) {
            GameObject lobbySingleGO = Instantiate(lobbyPanelPrefab, lobbyListContainer);
            LobbyGOController lobbyGO = lobbySingleGO.GetComponent<LobbyGOController>();
            lobbyGO.InitData(this);
            lobbyList.Add(lobbyGO);
            lobbySingleGO.SetActive(false);
        }
    }

    public void UpdateLobbyList(object sender, LobbyController.OnLobbyListChangedEventArgs e) {
        HideAllLobbiesList();
        for (int i = 0; i < e.lobbyList.Count; i++) {
            lobbyList[i].ShowUpdateLobbyData(e.lobbyList[i]);
        }
    }

    public void HideAllLobbiesList() {
        foreach (LobbyGOController lobbyGOController in lobbyList) {
            lobbyGOController.HideLobby();
        }
    }

    public void HideCanvas() {
        gameObject.SetActive(false);
    }

    public void ShowCanvas() {
        gameObject.SetActive(true);
        LobbyController.Instance.ListLobbies();
    }

    public void JoinLobby() {
        HideCanvas();
        playerListUI.ShowCanvas();
    }
}
