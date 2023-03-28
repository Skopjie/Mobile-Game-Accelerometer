using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSingleGOController : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] TextMeshProUGUI namePlayerText;
    [SerializeField] GameObject kickButton;

    [Header("Botones")]
    [SerializeField] Button kickPlayerButton;

    Player player;

    private void Awake() {
        kickPlayerButton.onClick.AddListener(() => {
            KickPlayer();
        });
    }

    public void UpdateLobbyData(Player playerData, bool isKickVisable)
    {
        player = playerData;
        gameObject.SetActive(true);
        kickButton.SetActive(isKickVisable);
        namePlayerText.text = playerData.Data["PlayerName"].Value;
    }

    public void KickPlayer() {
        if(player != null) {
            LobbyController.Instance.KickPlayers(player.Id);
        }
    }

    public void HidePlayerGO() {
        gameObject.SetActive(false);
        kickButton.SetActive(false);
    }
}
