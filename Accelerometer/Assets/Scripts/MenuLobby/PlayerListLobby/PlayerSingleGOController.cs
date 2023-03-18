using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class PlayerSingleGOController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI namePlayerText;

    public void UpdateLobbyData(Player playerData)
    {
        namePlayerText.text = playerData.Data["PlayerName"].Value;
    }
}
