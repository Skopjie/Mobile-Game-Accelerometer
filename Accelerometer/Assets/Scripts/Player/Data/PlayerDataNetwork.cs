using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

public class PlayerDataNetwork : NetworkBehaviour
{
    public struct PlayerData : INetworkSerializable {
        public int idPlayer;
        public FixedString128Bytes namePlayer;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref idPlayer);
            serializer.SerializeValue(ref namePlayer);
        }
    }

    public NetworkVariable<PlayerData> playerData = new NetworkVariable<PlayerData>(
        new PlayerData {
            idPlayer = 5,
            namePlayer = ""
        }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [SerializeField] int _idPlayer;
    [SerializeField] string _namePlayer;

    public List<Vector3> positionsSpawnPlayersList = new List<Vector3>();

    void Start() {
        SetPlayerData(RelayController.Instance.relayData);

        GameStateManager.Instance.playersNetwork.Add(gameObject.GetComponent<NetworkObject>());
        GameStateManager.Instance.AddNewPlayerClientRpc();

        if (IsOwner)
            InitializeTable();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("gameOver")) {
            Debug.Log("Perdio el jugador: " + OwnerClientId);

            if (IsHost)
                GameStateManager.Instance.PlayerEliminated();

            Invoke("CheckFinalGame", 0.5f);
        }
    }

    void InitializeTable() {
        if (IsOwner && OwnerClientId == 0) {
            Debug.Log("Creo tablero");
            TableMap.Instance.InitTable();
        }

        else {
            Debug.Log("Inicializo tablero");
            TableMap.Instance.AddSquareToData();
        }
    }

    void CheckFinalGame() {
        GameStateManager.Instance.CheckNumberOfPlayersClientRpc();
    }

    public void SetPlayerData(LobbyPlayerRelayData newRelayData) {
        if(IsOwner) {
            playerData.Value = new PlayerData {
                idPlayer = (int)OwnerClientId,
                namePlayer = newRelayData.namePlayer
            };

            _idPlayer = (int)OwnerClientId;
            _namePlayer = newRelayData.namePlayer;
        }
    }

    public Vector3 GetPositionSpawn() {
        SetPlayerData(RelayController.Instance.relayData);
        return positionsSpawnPlayersList[playerData.Value.idPlayer];
    }
}
