using Unity.Netcode;
using UnityEngine;

public class AccelerometerInputNetwork : NetworkBehaviour
{
    [Header("Componentes")]
    [SerializeField] Rigidbody rgbd;
    [SerializeField] PlayerDataNetwork playerData;
    [SerializeField] MeshRenderer meshRender;

    [Header("Variables")]
    [SerializeField] float forceSpeed;

    void Start() {
        InitComponents();

        GameStateManager.Instance.OnRoundIsOver += SetPosition;
        GameStateManager.Instance.OnAllPlayerConnected += SetPosition;
        GameStateManager.Instance.OnRoundStart += ActiveMovement;
    }

    void FixedUpdate() {
        if (!IsOwner) return;
        GetAccelerometer();
    }

    void InitComponents() {
        if (rgbd == null)
            rgbd = GetComponent<Rigidbody>();

        if (meshRender == null)
            meshRender = GetComponent<MeshRenderer>();

        if (playerData == null)
            playerData = GetComponent<PlayerDataNetwork>();
    }

    void ActiveMovement(object sender, GameStateManager.GameStateEventArgs e) {
        rgbd.isKinematic = false;
    }

    void DesactiveMovement() {
        rgbd.isKinematic = true;
    }

    void SetPosition(object sender, GameStateManager.GameStateEventArgs e) {
        rgbd = GetComponent<Rigidbody>();
        if (IsOwner) {
            rgbd.velocity = new Vector3(0, 0, 0);
            transform.position = playerData.GetPositionSpawn();
            DesactiveMovement();
        }
    }

    void GetAccelerometer() {
        Vector3 tilt = Input.acceleration * forceSpeed;
        tilt = Quaternion.Euler(90, 0, 0) * tilt;

        rgbd.AddForce(tilt);
    }
}
