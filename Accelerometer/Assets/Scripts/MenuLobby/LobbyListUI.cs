using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine.UI;
using UnityEngine;

public class LobbyListUI : MonoBehaviour
{
    public static LobbyListUI Instance { get; private set; }

    [Header("Componentes")]
    [SerializeField] Transform lobbyListContainer;
    [SerializeField] GameObject lobbyPanelPrefab;

    [Header("Botones")]
    [SerializeField] Button refreshLobbyListButton;
    [SerializeField] Button exitLobbyListButton;

    List<LobbyGOController> lobbyList;

    private void Awake() {
        Instance = this;

        refreshLobbyListButton.onClick.AddListener(() => {
            LobbyController.Instance.ListLobbies();
        });

        exitLobbyListButton.onClick.AddListener(() => {
            HideLobbyListCanvas();
        });
    }

    private void Start() {
        InitLobbyList();

        LobbyController.Instance.OnLobbyListChanged += UpdateLobbyList;
    }

    void InitLobbyList() {
        for (int i = 0; i < 25; i++) {
            GameObject lobbySingleGO = Instantiate(lobbyPanelPrefab, lobbyListContainer);
            lobbyList.Add(lobbySingleGO.GetComponent<LobbyGOController>());
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

    public void HideLobbyListCanvas() {
        gameObject.SetActive(false);
    }
}
