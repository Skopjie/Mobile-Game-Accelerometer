using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ConnectMultiplayerUI : MonoBehaviour
{
    string namePlayer;

    [Header("Componentes")]
    [SerializeField] TMP_InputField namePlayerInputField;
    [SerializeField] MultiplayerUI multiplayerUI;

    [Header("Botones")]
    [SerializeField] Button connectMultiplayerButton;

    private void Awake() {
        connectMultiplayerButton.onClick.AddListener(() => {
            SetPlayerName();
            LobbyController.Instance.AuthenticatioMultiplayer(namePlayer);
            LobbyUIController.Instance.ShowMultiplayerOptions();
        });
    }

    public void SetPlayerName() {
        namePlayer = namePlayerInputField.text;
    }

    public void HideCanvas() {
        gameObject.SetActive(false);
    }
    public void ShowCanvas() {
        gameObject.SetActive(true);
    }

}
