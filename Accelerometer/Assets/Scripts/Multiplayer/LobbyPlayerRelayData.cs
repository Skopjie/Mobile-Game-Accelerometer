using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Relay.Models;

[CreateAssetMenu(fileName = "RelayData", menuName = "ScriptableObjects/RelayData")]
public class LobbyPlayerRelayData : ScriptableObject {
    public Allocation allocation;
    public string codeRelay;
    public string namePlayer;
    public int numberPlayer;
    public bool isHost;
}
