using Unity.Netcode;
using UnityEngine;

public class SquareNetwork : NetworkBehaviour {
    public NetworkVariable<Vector2> idSquare =
    new NetworkVariable<Vector2>(new Vector2(0, 0),
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    public NetworkVariable<Vector3> squareInitialPosition =
    new NetworkVariable<Vector3>(new Vector3(0, 0, 0),
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    public Vector2 GetIdSquare() {
        return idSquare.Value;
    }
    public Vector3 GetInitialPositionSquare() {
        return squareInitialPosition.Value;
    }
    public void SetPositionSquare(Vector3 newSquareInitialPosition) {
        squareInitialPosition.Value = newSquareInitialPosition;
    }
    public void SetIdSquare(Vector2 newId) {
        idSquare.Value = newId;
    }
    public void InitDataSquare(Vector2 newId, Vector3 newSquareInitialPosition) {
        idSquare.Value = newId;
        squareInitialPosition.Value = newSquareInitialPosition;
    }
}
