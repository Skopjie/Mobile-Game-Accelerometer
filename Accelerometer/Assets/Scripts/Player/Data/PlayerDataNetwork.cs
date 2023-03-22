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
   
    // Start is called before the first frame update
    void Start()
    {
        SetPlayerData(RelayController.Instance.relayData);
    }

    public void SetPlayerData(LobbyPlayerRelayData newRelayData) {
        playerData.Value = new PlayerData {
            idPlayer = (int)OwnerClientId,
            namePlayer = newRelayData.namePlayer
        };

        _idPlayer = (int)OwnerClientId;
        _namePlayer = newRelayData.namePlayer;
    }

    public Vector3 GetPositionSpawn() {
        SetPlayerData(RelayController.Instance.relayData);
        return positionsSpawnPlayersList[playerData.Value.idPlayer];
    }
}
