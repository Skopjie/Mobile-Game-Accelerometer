using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateLobbyUI : MonoBehaviour
{
    const string PRIVATETEXT = "Private";
    const string PUBLICTEXT = "Public";

    [Header("Componentes")]
    [SerializeField] PlayerListUI playerListUI;

    [Header("Textos")]
    [SerializeField] TMP_InputField inputNameLobbyText;
    [SerializeField] TextMeshProUGUI protectionLobbyText;
    [SerializeField] TextMeshProUGUI numPlayerText;

    [Header("Botones")]
    [SerializeField] Button createLobbyButton;
    [SerializeField] Button exitCreateLobbyButton;
    [SerializeField] Button protectionLobbyButton;
    [SerializeField] Button maxPlayersLobbyButton;

    [SerializeField] LobbyController lobbyControl;

    LobbyInfo lobbyInfo;

    private void Awake() {
        ResetLobbyData();

        createLobbyButton.onClick.AddListener(() => {
            Debug.Log(lobbyInfo.nameLobby + " / " + lobbyInfo.numPlayer + " / " + lobbyInfo.IsPrivate);
            LobbyController.Instance.CreateLobby(lobbyInfo);
            HideCanvas();
            playerListUI.ShowCanvas();
        });

        exitCreateLobbyButton.onClick.AddListener(() => {
            HideCanvas();
        });

        inputNameLobbyText.onValueChanged.AddListener((string newValue) => {
            ChangeNameLobby(newValue);
        });

        protectionLobbyButton.onClick.AddListener(() => {
            ChangeTypeProtectionLobby();
        });

        maxPlayersLobbyButton.onClick.AddListener(() => {
            ChangeNumPlayerLobby();
        });
    }


    public void ChangeNameLobby(string newNameLobby) {
        lobbyInfo.nameLobby = newNameLobby;
        UpdateText();
    }

    public void ChangeTypeProtectionLobby() {
        lobbyInfo.IsPrivate = !lobbyInfo.IsPrivate;
        UpdateText();
    }

    public void ChangeNumPlayerLobby() {
        lobbyInfo.numPlayer++;
        lobbyInfo.numPlayer = lobbyInfo.numPlayer == 5 ? 2 : lobbyInfo.numPlayer;
        UpdateText();
    }

    public void UpdateText() {
        inputNameLobbyText.text = lobbyInfo.nameLobby;
        numPlayerText.text = "" + lobbyInfo.numPlayer;
        protectionLobbyText.text = lobbyInfo.IsPrivate ? PRIVATETEXT : PUBLICTEXT;
    }

    public void ResetLobbyData() {
        lobbyInfo.nameLobby = "";
        lobbyInfo.numPlayer = 2;
        lobbyInfo.IsPrivate = false;
    }

    public void HideCanvas() {
        gameObject.SetActive(false);
    }

    public void ShowCanvas() { 
        gameObject.SetActive(true);

        ResetLobbyData();
        UpdateText();
    }
}
