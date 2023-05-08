using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour {
    void Start() {
        GameStateManager.Instance.playersNetwork.Add(gameObject.GetComponent<NetworkObject>());
        GameStateManager.Instance.AddNewPlayerClientRpc();

        if(IsOwner)
            InitializeTable();
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
}
